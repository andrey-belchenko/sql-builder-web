using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using sql.builder;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Handles dependency loading for database objects. Processes unprocessed items from db_objects table,
    /// extracts dependencies from views, materialized views, and procedures, and recursively processes
    /// newly discovered dependencies until no new items are found.
    /// </summary>
    public static class DbObjectDependencyLoader
    {
        /// <summary>
        /// Main entry point for dependency loading. Processes unprocessed database objects recursively
        /// until no new dependencies are found.
        /// </summary>
        /// <param name="customQuery">Optional SQL query for custom filtering of unprocessed items.
        /// Should select from report_dev_sqlb.db_objects table. Example:
        /// "SELECT object_name, object_type, processed FROM report_dev_sqlb.db_objects WHERE processed = false AND object_type = 'view'"
        /// If null, processes all unprocessed items.</param>
        public static void LoadDependencies(string customQuery = null)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Starting dependency loading...");
            if (!string.IsNullOrWhiteSpace(customQuery))
            {
                Console.WriteLine($"Using custom query for filtering unprocessed items.");
            }

            int iteration = 0;
            while (true)
            {
                iteration++;
                Console.WriteLine($"\n=== Iteration {iteration} ===");

                var unprocessed = AnalyzerStorage.GetUnprocessedDbObjects(customQuery);
                if (unprocessed.Count == 0)
                {
                    Console.WriteLine("No unprocessed items found. Dependency loading complete.");
                    break;
                }

                Console.WriteLine($"Found {unprocessed.Count} unprocessed items to process.");

                int processedCount = 0;
                int errorCount = 0;

                foreach (var dbObject in unprocessed)
                {
                    try
                    {
                        Console.WriteLine($"Processing: {dbObject.ObjectName} (Type: {dbObject.ObjectType?.ToString() ?? "null"})");
                        ProcessDbObject(dbObject);
                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        Console.WriteLine($"ERROR processing {dbObject.ObjectName}: {ex.Message}");
                        // Throw exception instead of just logging
                        throw;
                    }
                }

                Console.WriteLine($"Iteration {iteration} complete: {processedCount} processed, {errorCount} errors.");

                // Fix schemas after each iteration
                Console.WriteLine("Fixing schemas...");
                AnalyzerStorage.FixSchemas();
            }

            Console.WriteLine("\nDependency loading finished.");
        }

        /// <summary>
        /// Processes a single database object: resolves type, extracts dependencies, and marks as processed.
        /// </summary>
        private static void ProcessDbObject(AnalyzerDbObject dbObject)
        {
            if (string.IsNullOrEmpty(dbObject.ObjectName))
            {
                throw new ArgumentException("Object name cannot be null or empty");
            }

            DbObjectType? currentType = dbObject.ObjectType;

            // Step 1: If type is TableOrView, resolve the real type
            if (currentType == DbObjectType.TableOrView)
            {
                try
                {
                    var tableInfo = DevOracleSheme.GetTableInfo(dbObject.ObjectName);
                    currentType = tableInfo.Type;

                    // Update the database with the resolved type
                    AnalyzerStorage.UpdateDbObjectType(dbObject.ObjectName, tableInfo.Type);
                    Console.WriteLine($"  Resolved type: {currentType}");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to resolve type for '{dbObject.ObjectName}': {ex.Message}", ex);
                }
            }

            // Step 2: Process based on resolved type
            if (currentType == DbObjectType.Table)
            {
                // Tables have no dependencies to extract, just mark as processed
                AnalyzerStorage.UpdateDbObjectProcessed(dbObject.ObjectName, true);
                Console.WriteLine($"  Table - no dependencies to extract");
            }
            else if (currentType == DbObjectType.TempTable)
            {
                // Temp tables have no dependencies to extract, just mark as processed
                AnalyzerStorage.UpdateDbObjectProcessed(dbObject.ObjectName, true);
                Console.WriteLine($"  Temp table - no dependencies to extract");
            }
            else if (currentType.HasValue && (currentType.Value == DbObjectType.View || currentType.Value == DbObjectType.MatView))
            {
                ProcessViewOrMatView(dbObject.ObjectName, currentType.Value);
                AnalyzerStorage.UpdateDbObjectProcessed(dbObject.ObjectName, true);
            }
            else if (currentType == DbObjectType.Procedure)
            {
                ProcessProcedure(dbObject.ObjectName);
                AnalyzerStorage.UpdateDbObjectProcessed(dbObject.ObjectName, true);
            }
            else
            {
                // Unknown type or null - mark as processed to avoid infinite loop
                Console.WriteLine($"  Unknown type - marking as processed");
                AnalyzerStorage.UpdateDbObjectProcessed(dbObject.ObjectName, true);
            }
        }

        /// <summary>
        /// Processes a view or materialized view: gets DDL and extracts dependencies.
        /// </summary>
        private static void ProcessViewOrMatView(string objectName, DbObjectType type)
        {
            try
            {
                var tableInfo = DevOracleSheme.GetTableInfo(objectName);
                if (string.IsNullOrEmpty(tableInfo.DDL))
                {
                    Console.WriteLine($"  No DDL available for {objectName}");
                    return;
                }

                Console.WriteLine($"  Extracting dependencies from DDL...");
                var dependencies = ExtractDependenciesFromSql(tableInfo.DDL, objectName, type);

                if (System.Linq.Enumerable.Any(dependencies))
                {
                    var result = AnalyzerStorage.SaveDependencies(dependencies);
                    Console.WriteLine($"  Found {dependencies.Count()} dependencies ({result.NewDependencies.Count} new, {result.ExistingDependencies.Count} existing):");

                    if (System.Linq.Enumerable.Any(result.NewDependencies))
                    {
                        Console.WriteLine($"    New dependencies:");
                        foreach (var dep in result.NewDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                        {
                            bool isNewDbObject = result.NewDbObjects.Contains(dep.UsedObjectName, StringComparer.OrdinalIgnoreCase);
                            string dbObjectStatus = isNewDbObject ? " [NEW db_object]" : " [existing db_object]";
                            Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"}){dbObjectStatus}");
                        }
                    }

                    if (System.Linq.Enumerable.Any(result.ExistingDependencies))
                    {
                        Console.WriteLine($"    Existing dependencies:");
                        foreach (var dep in result.ExistingDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                        {
                            Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"  No dependencies found");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process view/matview '{objectName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Processes a procedure: handles both standalone procedures and package procedures.
        /// </summary>
        private static void ProcessProcedure(string objectName)
        {
            try
            {
                int lastDotIndex = objectName.LastIndexOf('.');

                if (lastDotIndex < 0)
                {
                    // Standalone procedure - no dot in name
                    Console.WriteLine($"  Standalone procedure: {objectName}");

                    try
                    {
                        var procedureInfo = DevOracleSheme.GetProcedureInfo(objectName);
                        if (string.IsNullOrEmpty(procedureInfo.DDL))
                        {
                            Console.WriteLine($"  No DDL available for procedure {objectName}");
                            return;
                        }

                        Console.WriteLine($"  Extracting dependencies from procedure DDL...");
                        // Extract dependencies directly (no procedureName parameter needed)
                        var dependencies = ExtractDependenciesFromSql(procedureInfo.DDL, objectName, DbObjectType.Procedure);

                        if (System.Linq.Enumerable.Any(dependencies))
                        {
                            var result = AnalyzerStorage.SaveDependencies(dependencies);
                            Console.WriteLine($"  Found {dependencies.Count()} dependencies ({result.NewDependencies.Count} new, {result.ExistingDependencies.Count} existing):");

                            if (System.Linq.Enumerable.Any(result.NewDependencies))
                            {
                                Console.WriteLine($"    New dependencies:");
                                foreach (var dep in result.NewDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                                {
                                    bool isNewDbObject = result.NewDbObjects.Contains(dep.UsedObjectName, StringComparer.OrdinalIgnoreCase);
                                    string dbObjectStatus = isNewDbObject ? " [NEW db_object]" : " [existing db_object]";
                                    Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"}){dbObjectStatus}");
                                }
                            }

                            if (System.Linq.Enumerable.Any(result.ExistingDependencies))
                            {
                                Console.WriteLine($"    Existing dependencies:");
                                foreach (var dep in result.ExistingDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                                {
                                    Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"})");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  No dependencies found");
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // If standalone procedure not found, try as package
                        Console.WriteLine($"  Not found as standalone procedure, trying as package...");
                        ProcessPackageProcedure(objectName, null);
                    }
                }
                else
                {
                    // Package procedure - has dot
                    string packageName = objectName.Substring(0, lastDotIndex);
                    string procedureName = objectName.Substring(lastDotIndex + 1);
                    ProcessPackageProcedure(packageName, procedureName);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to process procedure '{objectName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Processes a package procedure: gets package DDL and extracts dependencies for the specific procedure.
        /// </summary>
        private static void ProcessPackageProcedure(string packageName, string procedureName)
        {
            Console.WriteLine($"  Package: {packageName}, Procedure: {procedureName ?? "ALL"}");

            var packageInfo = DevOracleSheme.GetPackageInfo(packageName);
            if (string.IsNullOrEmpty(packageInfo.DDL))
            {
                Console.WriteLine($"  No DDL available for package {packageName}");
                return;
            }

            Console.WriteLine($"  Extracting dependencies from package DDL...");
            var objectName = procedureName != null ? $"{packageName}.{procedureName}" : packageName;
            var dependencies = ExtractDependenciesFromSql(packageInfo.DDL, objectName, DbObjectType.Procedure, procedureName);

            if (System.Linq.Enumerable.Any(dependencies))
            {
                var result = AnalyzerStorage.SaveDependencies(dependencies);
                Console.WriteLine($"  Found {dependencies.Count()} dependencies ({result.NewDependencies.Count} new, {result.ExistingDependencies.Count} existing):");

                if (System.Linq.Enumerable.Any(result.NewDependencies))
                {
                    Console.WriteLine($"    New dependencies:");
                    foreach (var dep in result.NewDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                    {
                        bool isNewDbObject = result.NewDbObjects.Contains(dep.UsedObjectName, StringComparer.OrdinalIgnoreCase);
                        string dbObjectStatus = isNewDbObject ? " [NEW db_object]" : " [existing db_object]";
                        Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"}){dbObjectStatus}");
                    }
                }

                if (System.Linq.Enumerable.Any(result.ExistingDependencies))
                {
                    Console.WriteLine($"    Existing dependencies:");
                    foreach (var dep in result.ExistingDependencies.OrderBy(d => d.UsedObjectType?.ToString() ?? "").ThenBy(d => d.UsedObjectName))
                    {
                        Console.WriteLine($"      - {dep.UsedObjectName} ({dep.UsedObjectType?.ToString() ?? "null"})");
                    }
                }
            }
            else
            {
                Console.WriteLine($"  No dependencies found");
            }
        }

        /// <summary>
        /// Extracts dependencies from SQL/DDL using DevSqlParserAntlr, similar to AnalyzeCmdSql.
        /// </summary>
        private static IEnumerable<AnalyzerDependency> ExtractDependenciesFromSql(string sql, string objectName, DbObjectType objectType, string procedureName = null)
        {
            var dependencies = new List<AnalyzerDependency>();

            if (string.IsNullOrEmpty(sql))
                return dependencies;

            try
            {
                // Clean SQL similar to AnalyzeCmdSql
                var cleanSql = Cmn.ClearUndefined(sql);
                // Replace "as end" alias when followed by non-alphanumeric character
                cleanSql = Regex.Replace(cleanSql, @"\bas\s+end(?![a-zA-Z0-9_])", "as \"end\"", RegexOptions.IgnoreCase);
                cleanSql = cleanSql.Replace("stragg_dist", "max");
                cleanSql = cleanSql.Replace("stragg", "max");

                // Extract table names
                GetSourceTablesResult tableResult;
                if (!string.IsNullOrEmpty(procedureName))
                {
                    tableResult = DevSqlParserAntlr.GetSourceTables(cleanSql, sql, procedureName);
                }
                else
                {
                    tableResult = DevSqlParserAntlr.GetSourceTables(cleanSql, sql, null);
                }

                // Extract procedure names
                HashSet<string> procNames;
                if (!string.IsNullOrEmpty(procedureName))
                {
                    procNames = DevSqlParserAntlr.GetSourceProcedures(cleanSql, sql, procedureName);
                }
                else
                {
                    procNames = DevSqlParserAntlr.GetSourceProcedures(cleanSql, sql, null);
                }

                // Create dependency records for tables/views
                foreach (var tableName in tableResult.TableNames)
                {
                    dependencies.Add(new AnalyzerDependency
                    {
                        ObjectName = objectName,
                        UsedObjectName = tableName,
                        UsedObjectType = DbObjectType.TableOrView
                    });
                }

                // Create dependency records for procedures
                foreach (var procName in procNames)
                {
                    dependencies.Add(new AnalyzerDependency
                    {
                        ObjectName = objectName,
                        UsedObjectName = procName,
                        UsedObjectType = DbObjectType.Procedure
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: Failed to parse SQL for {objectName}: {ex.Message}");
                // Throw exception instead of suppressing
                throw new InvalidOperationException($"Failed to parse SQL for '{objectName}': {ex.Message}", ex);
            }

            return dependencies;
        }
    }
}
