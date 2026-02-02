using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Npgsql;

namespace SqlBuilderLib.DevTools
{
    public static class LogsLoader
    {
        private static string ConnectionString = "Host=asusejs-dev.infoenergo.loc;Port=5432;Database=asuse;Username=asuse;Password=kl0pik";
        private static readonly object _lockObject = new object();
        private const string DateFormat = "dd.MM.yyyy HH:mm:ss";

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

        /// <summary>
        /// Extracts nav_id from filename pattern log-{nav_id}.csv or log-{nav_id}..csv
        /// </summary>
        private static string ExtractNavIdFromFileName(string fileName)
        {
            // Remove extension(s)
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName));
            
            // Match pattern: log-{nav_id}
            var match = Regex.Match(nameWithoutExtension, @"^log-(.+)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            
            return null;
        }

        /// <summary>
        /// Parses a CSV line with quoted fields
        /// Format: "user_name","reports_name","started_at","finished_at"
        /// </summary>
        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            bool inQuotes = false;
            string currentField = "";
            
            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }
            
            // Add the last field
            fields.Add(currentField);
            
            return fields.ToArray();
        }

        /// <summary>
        /// Parses a date string in format "dd.MM.yyyy HH:mm:ss"
        /// </summary>
        private static DateTime? ParseDateTime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;
            
            // Remove quotes if present
            dateString = dateString.Trim('"');
            
            if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            
            return null;
        }

        /// <summary>
        /// Loads log data from CSV files matching pattern log-*.csv in the Data folder
        /// Clears the reports_exec table before loading
        /// </summary>
        public static void LoadLogsFromCsvFiles()
        {
            lock (_lockObject)
            {
                var dataFolder = GetDataFolderPath();
                var csvFiles = Directory.GetFiles(dataFolder, "log-*.csv", SearchOption.TopDirectoryOnly);
                
                if (csvFiles.Length == 0)
                {
                    return;
                }

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    
                    // Clear the table before loading
                    using (var truncateCommand = new NpgsqlCommand("TRUNCATE TABLE report_dev_sqlb.reports_exec", connection))
                    {
                        truncateCommand.ExecuteNonQuery();
                    }

                    // Process each CSV file
                    int totalRecordsProcessed = 0;
                    foreach (var csvFile in csvFiles)
                    {
                        string navId = ExtractNavIdFromFileName(csvFile);
                        if (string.IsNullOrEmpty(navId))
                        {
                            Console.WriteLine($"Skipping file '{csvFile}' - nav_id could not be extracted from filename");
                            continue; // Skip files that don't match the pattern
                        }

                        Console.WriteLine($"Processing file: {csvFile} (nav_id: {navId})");
                        var lines = File.ReadAllLines(csvFile);
                        int fileRecordCount = 0;
                        
                        foreach (var line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            var fields = ParseCsvLine(line);
                            
                            // Expected format: user_name, reports_name, started_at, finished_at
                            if (fields.Length < 4)
                            {
                                Console.WriteLine($"Skipping line - insufficient fields (expected 4, got {fields.Length}): {line}");
                                continue;
                            }

                            string userName = fields[0].Trim('"');
                            string reportsName = fields[1].Trim('"');
                            DateTime? startedAt = ParseDateTime(fields[2]);
                            DateTime? finishedAt = ParseDateTime(fields[3]);

                            // Insert record
                            using (var command = new NpgsqlCommand())
                            {
                                command.Connection = connection;
                                command.CommandText = "INSERT INTO report_dev_sqlb.reports_exec (nav_id, reports_name, user_name, started_at, finished_at) VALUES (@nav_id, @reports_name, @user_name, @started_at, @finished_at)";
                                
                                command.Parameters.AddWithValue("@nav_id", navId ?? (object)DBNull.Value);
                                command.Parameters.AddWithValue("@reports_name", string.IsNullOrEmpty(reportsName) ? (object)DBNull.Value : reportsName);
                                command.Parameters.AddWithValue("@user_name", string.IsNullOrEmpty(userName) ? (object)DBNull.Value : userName);
                                command.Parameters.AddWithValue("@started_at", startedAt.HasValue ? (object)startedAt.Value : DBNull.Value);
                                command.Parameters.AddWithValue("@finished_at", finishedAt.HasValue ? (object)finishedAt.Value : DBNull.Value);
                                
                                command.ExecuteNonQuery();
                            }

                            totalRecordsProcessed++;
                            fileRecordCount++;
                            
                            // Log progress every 100 records
                            if (totalRecordsProcessed % 100 == 0)
                            {
                                Console.WriteLine($"Processed {totalRecordsProcessed} records...");
                            }
                        }
                        
                        Console.WriteLine($"Completed file '{csvFile}': {fileRecordCount} records processed");
                    }
                    
                    Console.WriteLine($"Total records loaded: {totalRecordsProcessed}");
                }
            }
        }
    }
}
