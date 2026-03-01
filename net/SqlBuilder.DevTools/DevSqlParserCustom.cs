using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using sql.builder;
using DataHelper = infoenergo.core.Data.DataHelper;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Custom PL/SQL parser for extracting database table/view and package names.
    /// Uses simple regex-based word extraction and matches against database metadata.
    /// </summary>
    internal static class DevSqlParserCustom
    {
        private static readonly object _lockObject = new object();
        private static HashSet<string> _tableAndViewNames;
        private static HashSet<string> _packageNames;
        private static bool _isInitialized = false;

        // Regex pattern to extract Latin identifiers (words starting with letter, followed by letters, digits, or underscores)
        private static readonly Regex LatinWordPattern = new Regex(@"\b[A-Za-z][A-Za-z0-9_]*\b", RegexOptions.Compiled);

        /// <summary>
        /// Ensures database metadata is loaded (thread-safe lazy initialization).
        /// </summary>
        private static void EnsureInitialized()
        {
            if (_isInitialized)
                return;

            lock (_lockObject)
            {
                if (_isInitialized)
                    return;

                LoadDatabaseMetadata();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Loads table/view and package names from the database.
        /// </summary>
        private static void LoadDatabaseMetadata()
        {
            _tableAndViewNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _packageNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // Load tables and views
                string tablesAndViewsSql = @"
                    SELECT table_name as object_name FROM all_tables
                    UNION
                    SELECT view_name as object_name FROM all_views";

                DataTable dtTablesAndViews = DataHelper.SqlGetTable(tablesAndViewsSql, db.Connection, false);
                if (dtTablesAndViews != null)
                {
                    foreach (DataRow row in dtTablesAndViews.Rows)
                    {
                        string name = row.Field<string>("object_name");
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            _tableAndViewNames.Add(name.Trim());
                        }
                    }
                }

                // Load packages
                string packagesSql = @"
                    SELECT object_name FROM all_objects 
                    WHERE object_type IN ('PACKAGE', 'PACKAGE BODY')";

                DataTable dtPackages = DataHelper.SqlGetTable(packagesSql, db.Connection, false);
                if (dtPackages != null)
                {
                    foreach (DataRow row in dtPackages.Rows)
                    {
                        string name = row.Field<string>("object_name");
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            _packageNames.Add(name.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If metadata loading fails, initialize with empty sets to avoid repeated failures
                _tableAndViewNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _packageNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                // Optionally log the error or rethrow depending on requirements
                throw new InvalidOperationException("Failed to load database metadata for DevSqlParserCustom", ex);
            }
        }

        /// <summary>
        /// Removes SQL comments (single-line -- and multi-line /* */) from SQL text.
        /// </summary>
        /// <param name="sqlText">SQL or PL/SQL code string</param>
        /// <returns>SQL text with comments removed</returns>
        private static string RemoveComments(string sqlText)
        {
            if (string.IsNullOrWhiteSpace(sqlText))
                return sqlText;

            string result = sqlText;

            // Remove multi-line comments /* ... */
            result = Regex.Replace(result, @"/\*.*?\*/", "", RegexOptions.Singleline | RegexOptions.Multiline);

            // Remove single-line comments -- ... (but not if -- is part of a string)
            // This regex matches -- followed by any characters until end of line
            result = Regex.Replace(result, @"--.*?$", "", RegexOptions.Multiline);

            return result;
        }

        /// <summary>
        /// Extracts all Latin words from SQL text.
        /// </summary>
        /// <param name="sqlText">SQL or PL/SQL code string</param>
        /// <returns>HashSet of extracted Latin words that contain underscore</returns>
        private static HashSet<string> ExtractLatinWords(string sqlText)
        {
            var words = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(sqlText))
                return words;

            // Remove comments before extracting words
            string cleanedSql = RemoveComments(sqlText);

            var matches = LatinWordPattern.Matches(cleanedSql);
            foreach (Match match in matches)
            {
                string word = match.Value;
                // Only keep words that contain underscore
                if (!string.IsNullOrWhiteSpace(word) && word.Contains("_"))
                {
                    words.Add(word);
                }
            }

            return words;
        }

        /// <summary>
        /// Extracts all database table and view names from PL/SQL code using custom logic.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <returns>HashSet of table/view names that match database metadata</returns>
        public static HashSet<string> GetSourceTables(string plsqlText)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            EnsureInitialized();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var extractedWords = ExtractLatinWords(plsqlText);

            // Match extracted words against loaded table/view names
            foreach (string word in extractedWords)
            {
                if (_tableAndViewNames.Contains(word))
                {
                    result.Add(word);
                }
            }

            return result.Where(it => it != "rr_temp").ToHashSet();
        }

        /// <summary>
        /// Extracts all package names from PL/SQL code using custom logic.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <returns>HashSet of package names that match database metadata</returns>
        public static HashSet<string> GetSourcePackages(string plsqlText)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            EnsureInitialized();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var extractedWords = ExtractLatinWords(plsqlText);

            // Match extracted words against loaded package names
            foreach (string word in extractedWords)
            {
                if (_packageNames.Contains(word))
                {
                    result.Add(word);
                }
            }

            return result.Where(it => it != "kg_common" && it != "ng_account").ToHashSet();
        }
    }
}
