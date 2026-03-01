using System;
using System.Collections.Generic;
using System.Data;
using sql.builder.Clean;
using sql.builder;
using DataHelper = infoenergo.core.Data.DataHelper;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Information about a table, view, or materialized view.
    /// </summary>
    public class TableInfo
    {
        public string Name { get; set; }
        public DbObjectType Type { get; set; }
        public string DDL { get; set; } // null for tables, DDL for views and mat views
    }

    /// <summary>
    /// Information about a package.
    /// </summary>
    public class PackageInfo
    {
        public string Name { get; set; }
        public string DDL { get; set; } // PACKAGE BODY DDL
    }

    /// <summary>
    /// Static class for retrieving Oracle database schema information (tables, views, materialized views, packages) with DDL retrieval and caching.
    /// Compatible with Oracle 11g.
    /// </summary>
    public static class DevOracleSheme
    {
        private static readonly Dictionary<string, TableInfo> _tableCache = new Dictionary<string, TableInfo>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, PackageInfo> _packageCache = new Dictionary<string, PackageInfo>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Gets information about a table, view, or materialized view.
        /// Returns cached information if available.
        /// </summary>
        /// <param name="objectName">Name of the table, view, or materialized view</param>
        /// <returns>TableInfo with Name, Type, and DDL (for views and mat views)</returns>
        /// <exception cref="InvalidOperationException">Thrown if object is not found</exception>
        public static TableInfo GetTableInfo(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                throw new ArgumentException("Object name cannot be null or empty", nameof(objectName));

            string cacheKey = objectName.ToUpper();

            if (objectName == "vv_day")
            {

            }

            // Check cache first
            lock (_lockObject)
            {
                if (_tableCache.TryGetValue(cacheKey, out TableInfo cachedInfo))
                {
                    return cachedInfo;
                }
            }

            // Not in cache, query database
            try
            {
                TableInfo info = QueryTableInfo(objectName);

                // Cache the result
                lock (_lockObject)
                {
                    _tableCache[cacheKey] = info;
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve table info for '{objectName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets information about a package.
        /// Returns cached information if available.
        /// </summary>
        /// <param name="packageName">Name of the package</param>
        /// <returns>PackageInfo with Name and DDL (PACKAGE BODY)</returns>
        /// <exception cref="InvalidOperationException">Thrown if package is not found</exception>
        public static PackageInfo GetPackageInfo(string packageName)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                throw new ArgumentException("Package name cannot be null or empty", nameof(packageName));

            string cacheKey = packageName.ToUpper();

            // Check cache first
            lock (_lockObject)
            {
                if (_packageCache.TryGetValue(cacheKey, out PackageInfo cachedInfo))
                {
                    return cachedInfo;
                }
            }

            // Not in cache, query database
            try
            {
                PackageInfo info = QueryPackageInfo(packageName);

                // Cache the result
                lock (_lockObject)
                {
                    _packageCache[cacheKey] = info;
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve package info for '{packageName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Queries the database for table/view/materialized view information.
        /// </summary>
        private static TableInfo QueryTableInfo(string objectName)
        {
            VOracleParameter[] parameters = new VOracleParameter[]
            {
                new VOracleParameter("object_name", VOracleDbType.VarChar, objectName, ParameterDirection.Input)
            };

            // Check if it's a materialized view first
            bool isMaterializedView = false;
            string mviewSql = @"
                SELECT owner 
                FROM all_mviews 
                WHERE mview_name = UPPER(:object_name) 
                  AND owner = USER";

            DataTable mviewDt = DataHelper.SqlGetTable(mviewSql, parameters, db.Connection, false);
            string owner = null;
            if (mviewDt != null && mviewDt.Rows.Count > 0)
            {
                isMaterializedView = true;
                owner = mviewDt.Rows[0].Field<string>("owner");
            }

            // Query all_objects to get object type and owner
            string sql;
            if (isMaterializedView)
            {
                // For mat views, query MATERIALIZED VIEW object type
                sql = @"
                    SELECT owner, object_type, object_name 
                    FROM all_objects 
                    WHERE object_name = UPPER(:object_name) 
                      AND owner = USER
                      AND object_type = 'MATERIALIZED VIEW'";
            }
            else
            {
                sql = @"
                    SELECT owner, object_type, object_name 
                    FROM all_objects 
                    WHERE object_name = UPPER(:object_name) 
                      --AND owner = USER
                      AND object_type IN ('TABLE', 'VIEW')";
            }

            DataTable dt = DataHelper.SqlGetTable(sql, parameters, db.Connection, false);

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new InvalidOperationException($"Object '{objectName}' not found in current schema");
            }

            DataRow row = dt.Rows[0];
            if (owner == null)
            {
                owner = row.Field<string>("owner");
            }
            string objectType = row.Field<string>("object_type");
            string actualObjectName = row.Field<string>("object_name"); // Get actual object name from DB

            // Check if it's a temporary table
            bool isTempTable = false;
            if (objectType == "TABLE")
            {
                VOracleParameter[] tempParams = new VOracleParameter[]
                {
                    new VOracleParameter("object_name", VOracleDbType.VarChar, actualObjectName, ParameterDirection.Input),
                    new VOracleParameter("owner", VOracleDbType.VarChar, owner, ParameterDirection.Input)
                };

                string tempCheckSql = @"
                    SELECT temporary 
                    FROM all_tables 
                    WHERE table_name = UPPER(:object_name) 
                      AND owner = :owner";

                DataTable tempDt = DataHelper.SqlGetTable(tempCheckSql, tempParams, db.Connection, false);
                if (tempDt != null && tempDt.Rows.Count > 0)
                {
                    string temporary = tempDt.Rows[0].Field<string>("temporary");
                    isTempTable = temporary == "Y";
                }
            }

            // Determine type
            DbObjectType type;
            if (isMaterializedView)
            {
                type = DbObjectType.MatView;
            }
            else if (isTempTable)
            {
                type = DbObjectType.TempTable;
            }
            else if (objectType == "VIEW")
            {
                type = DbObjectType.View;
            }
            else
            {
                type = DbObjectType.Table;
            }

            // Get DDL for views and materialized views
            string ddl = null;
            if (type == DbObjectType.View || type == DbObjectType.MatView)
            {
                VOracleParameter[] ddlParameters = new VOracleParameter[]
                {
                    new VOracleParameter("object_name", VOracleDbType.VarChar, actualObjectName, ParameterDirection.Input),
                    new VOracleParameter("owner", VOracleDbType.VarChar, owner, ParameterDirection.Input)
                };

                string ddlType = type == DbObjectType.MatView ? "MATERIALIZED_VIEW" : "VIEW";
                string ddlSql = $"SELECT DBMS_METADATA.GET_DDL('{ddlType}', :object_name, :owner) FROM DUAL";
                ddl = DataHelper.SqlGetString(ddlSql, ddlParameters, db.Connection, false);
            }

            return new TableInfo
            {
                Name = objectName,
                Type = type,
                DDL = ddl
            };
        }

        /// <summary>
        /// Queries the database for package information.
        /// </summary>
        private static PackageInfo QueryPackageInfo(string packageName)
        {
            // Query all_objects to verify package exists and get owner
            VOracleParameter[] parameters = new VOracleParameter[]
            {
                new VOracleParameter("package_name", VOracleDbType.VarChar, packageName, ParameterDirection.Input)
            };

            string sql = @"
                SELECT owner, object_name 
                FROM all_objects 
                WHERE object_name = UPPER(:package_name) 
                  AND owner = USER
                  AND object_type = 'PACKAGE BODY'";

            DataTable dt = DataHelper.SqlGetTable(sql, parameters, db.Connection, false);

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new InvalidOperationException($"Package '{packageName}' not found in current schema");
            }

            DataRow row = dt.Rows[0];
            string owner = row.Field<string>("owner");
            string actualPackageName = row.Field<string>("object_name"); // Get actual package name from DB

            // Get PACKAGE BODY DDL
            VOracleParameter[] ddlParameters = new VOracleParameter[]
            {
                new VOracleParameter("package_name", VOracleDbType.VarChar, actualPackageName, ParameterDirection.Input),
                new VOracleParameter("owner", VOracleDbType.VarChar, owner, ParameterDirection.Input)
            };

            string ddlSql = "SELECT DBMS_METADATA.GET_DDL('PACKAGE_BODY', :package_name, :owner) FROM DUAL";
            string ddl = DataHelper.SqlGetString(ddlSql, ddlParameters, db.Connection, false);

            return new PackageInfo
            {
                Name = packageName,
                DDL = ddl
            };
        }

        /// <summary>
        /// Gets information about a standalone procedure.
        /// Returns cached information if available.
        /// </summary>
        /// <param name="procedureName">Name of the standalone procedure</param>
        /// <returns>PackageInfo with Name and DDL (PROCEDURE DDL)</returns>
        /// <exception cref="InvalidOperationException">Thrown if procedure is not found</exception>
        public static PackageInfo GetProcedureInfo(string procedureName)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("Procedure name cannot be null or empty", nameof(procedureName));

            string cacheKey = $"PROCEDURE_{procedureName.ToUpper()}";

            // Check cache first
            lock (_lockObject)
            {
                if (_packageCache.TryGetValue(cacheKey, out PackageInfo cachedInfo))
                {
                    return cachedInfo;
                }
            }

            // Not in cache, query database
            try
            {
                PackageInfo info = QueryProcedureInfo(procedureName);

                // Cache the result
                lock (_lockObject)
                {
                    _packageCache[cacheKey] = info;
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve procedure info for '{procedureName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Queries the database for standalone procedure information.
        /// </summary>
        private static PackageInfo QueryProcedureInfo(string procedureName)
        {
            // Query all_objects to verify procedure exists and get owner
            VOracleParameter[] parameters = new VOracleParameter[]
            {
                new VOracleParameter("procedure_name", VOracleDbType.VarChar, procedureName, ParameterDirection.Input)
            };

            string sql = @"
                SELECT owner, object_name 
                FROM all_objects 
                WHERE object_name = UPPER(:procedure_name) 
                  AND owner = USER
                  AND object_type = 'PROCEDURE'";

            DataTable dt = DataHelper.SqlGetTable(sql, parameters, db.Connection, false);

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new InvalidOperationException($"Procedure '{procedureName}' not found in current schema");
            }

            DataRow row = dt.Rows[0];
            string owner = row.Field<string>("owner");
            string actualProcedureName = row.Field<string>("object_name"); // Get actual procedure name from DB

            // Get PROCEDURE DDL
            VOracleParameter[] ddlParameters = new VOracleParameter[]
            {
                new VOracleParameter("procedure_name", VOracleDbType.VarChar, actualProcedureName, ParameterDirection.Input),
                new VOracleParameter("owner", VOracleDbType.VarChar, owner, ParameterDirection.Input)
            };

            string ddlSql = "SELECT DBMS_METADATA.GET_DDL('PROCEDURE', :procedure_name, :owner) FROM DUAL";
            string ddl = DataHelper.SqlGetString(ddlSql, ddlParameters, db.Connection, false);

            return new PackageInfo
            {
                Name = procedureName,
                DDL = ddl
            };
        }
    }
}
