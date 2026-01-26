using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Static class for parsing PL/SQL code and extracting database table names.
    /// Handles anonymous blocks, procedures, packages, CTEs, subqueries, and UNION statements.
    /// </summary>
    internal static class DevSqlParser
    {
        /// <summary>
        /// Extracts all database table names from PL/SQL code.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <returns>HashSet of table names (schema-qualified names are preserved)</returns>
        public static HashSet<string> GetSourceTables(string plsqlText)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Remove comments first to avoid false matches
            string cleanedSql = RemoveComments(plsqlText);
            
            // Extract all SELECT statements from PL/SQL
            var selectStatements = ExtractSelectStatements(cleanedSql);
            
            foreach (var selectSql in selectStatements)
            {
                var tables = ExtractTablesFromSelect(selectSql);
                foreach (var table in tables)
                {
                    if (!string.IsNullOrWhiteSpace(table))
                    {
                        result.Add(NormalizeTableName(table));
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Extracts all SELECT statements from PL/SQL code, including those in procedures, packages, and anonymous blocks.
        /// </summary>
        private static List<string> ExtractSelectStatements(string plsql)
        {
            var statements = new List<string>();
            
            if (string.IsNullOrWhiteSpace(plsql))
                return statements;

            // Handle EXECUTE IMMEDIATE statements that contain SQL
            var executeImmediateSql = ExtractSqlFromExecuteImmediate(plsql);
            statements.AddRange(executeImmediateSql);

            // Extract SELECT statements using regex pattern
            // Pattern matches SELECT ... FROM ... (handles multi-line)
            // We look for SELECT followed by something and then FROM to avoid matching SELECT in subqueries prematurely
            var selectPattern = new Regex(
                @"\bSELECT\s+",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline
            );

            // Find all SELECT statements
            var matches = selectPattern.Matches(plsql);
            var processedIndices = new HashSet<int>();
            
            foreach (Match match in matches)
            {
                // Skip if we've already processed this SELECT (might be part of a larger statement)
                if (processedIndices.Contains(match.Index))
                    continue;

                // Check if this SELECT has a FROM clause (to distinguish from SELECT in subqueries we'll process later)
                int fromIndex = FindFromClauseAfterSelect(plsql, match.Index);
                if (fromIndex < 0)
                    continue;

                // Extract the full SELECT statement
                string selectStatement = ExtractFullSelectStatement(plsql, match.Index);
                if (!string.IsNullOrWhiteSpace(selectStatement))
                {
                    statements.Add(selectStatement);
                    // Mark this index as processed
                    processedIndices.Add(match.Index);
                }
            }

            return statements;
        }

        /// <summary>
        /// Finds the FROM clause after a SELECT statement, skipping over subqueries and string literals.
        /// </summary>
        private static int FindFromClauseAfterSelect(string sql, int selectIndex)
        {
            if (selectIndex < 0 || selectIndex >= sql.Length)
                return -1;

            int depth = 0;
            bool inString = false;
            char stringChar = '\0';
            int i = selectIndex;

            // Skip past SELECT keyword
            while (i < sql.Length && char.IsLetterOrDigit(sql[i]))
                i++;

            while (i < sql.Length)
            {
                char c = sql[i];
                char next = i + 1 < sql.Length ? sql[i + 1] : '\0';

                // Handle string literals
                if (!inString && (c == '\'' || c == '"'))
                {
                    inString = true;
                    stringChar = c;
                }
                else if (inString && c == stringChar)
                {
                    if (c == '\'' && next == '\'')
                    {
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inString = false;
                    }
                }
                else if (!inString)
                {
                    if (c == '(')
                        depth++;
                    else if (c == ')')
                        depth--;
                    else if (depth == 0)
                    {
                        // Check for FROM keyword
                        if (i + 4 < sql.Length && 
                            sql.Substring(i, 5).Equals("FROM ", StringComparison.OrdinalIgnoreCase))
                        {
                            return i;
                        }
                    }
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Extracts the complete SELECT statement starting from the given index.
        /// Handles nested parentheses, subqueries, and UNION statements.
        /// </summary>
        private static string ExtractFullSelectStatement(string sql, int startIndex)
        {
            if (startIndex < 0 || startIndex >= sql.Length)
                return string.Empty;

            int depth = 0;
            bool inString = false;
            bool inSingleQuotedString = false;
            char stringChar = '\0';
            int selectStart = startIndex;
            int i = startIndex;

            // Find the start of SELECT keyword
            while (i < sql.Length && char.IsWhiteSpace(sql[i]))
                i++;

            // Track parentheses and strings
            while (i < sql.Length)
            {
                char c = sql[i];
                char next = i + 1 < sql.Length ? sql[i + 1] : '\0';

                // Handle string literals
                if (!inString && (c == '\'' || c == '"'))
                {
                    inString = true;
                    inSingleQuotedString = (c == '\'');
                    stringChar = c;
                }
                else if (inString && c == stringChar)
                {
                    // Check for escaped quotes ('')
                    if (inSingleQuotedString && next == '\'')
                    {
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inString = false;
                    }
                }
                else if (!inString)
                {
                    if (c == '(')
                        depth++;
                    else if (c == ')')
                    {
                        depth--;
                        if (depth < 0)
                            break;
                    }
                    // Check for end of statement (semicolon, UNION, or end of SQL block)
                    else if (depth == 0 && (c == ';' || 
                        (i + 5 < sql.Length && sql.Substring(i, 6).Equals("UNION ", StringComparison.OrdinalIgnoreCase)) ||
                        (i + 9 < sql.Length && sql.Substring(i, 10).Equals("UNION ALL ", StringComparison.OrdinalIgnoreCase))))
                    {
                        break;
                    }
                }

                i++;
            }

            if (i > selectStart)
            {
                return sql.Substring(selectStart, i - selectStart).Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts SQL statements from EXECUTE IMMEDIATE calls.
        /// </summary>
        private static List<string> ExtractSqlFromExecuteImmediate(string plsql)
        {
            var statements = new List<string>();
            
            // Pattern to match EXECUTE IMMEDIATE 'sql_string' or EXECUTE IMMEDIATE sql_variable
            var pattern = new Regex(
                @"EXECUTE\s+IMMEDIATE\s+('(?:''|[^'])*'|""(?:""""|[^""])*""|\w+)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var matches = pattern.Matches(plsql);
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    string sqlValue = match.Groups[1].Value.Trim();
                    // Remove quotes if present
                    if ((sqlValue.StartsWith("'") && sqlValue.EndsWith("'")) ||
                        (sqlValue.StartsWith("\"") && sqlValue.EndsWith("\"")))
                    {
                        sqlValue = sqlValue.Substring(1, sqlValue.Length - 2);
                        // Unescape quotes
                        sqlValue = sqlValue.Replace("''", "'").Replace("\"\"", "\"");
                    }
                    
                    // Check if it contains SELECT
                    if (sqlValue.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        statements.Add(sqlValue);
                    }
                }
            }

            return statements;
        }

        /// <summary>
        /// Extracts table names from a SELECT statement, handling CTEs, subqueries, and UNION statements.
        /// </summary>
        private static HashSet<string> ExtractTablesFromSelect(string selectSql)
        {
            var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(selectSql))
                return tables;

            // Extract CTE names to exclude them from results
            var cteNames = ExtractCteNames(selectSql);

            // Handle UNION statements - process each SELECT separately
            var unionParts = SplitUnionStatements(selectSql);
            
            foreach (var part in unionParts)
            {
                // Remove CTE clause if present (we already extracted CTE names)
                string sqlWithoutCte = RemoveCteClause(part);
                
                // Extract tables from FROM and JOIN clauses
                var fromTables = ExtractTablesFromFromClause(sqlWithoutCte);
                var joinTables = ExtractTablesFromJoinClause(sqlWithoutCte);
                
                foreach (var table in fromTables)
                {
                    if (!IsCteName(table, cteNames))
                        tables.Add(table);
                }
                
                foreach (var table in joinTables)
                {
                    if (!IsCteName(table, cteNames))
                        tables.Add(table);
                }
                
                // Extract tables from subqueries in SELECT, WHERE, HAVING clauses
                var subqueryTables = ExtractTablesFromSubqueries(sqlWithoutCte, cteNames);
                tables.UnionWith(subqueryTables);
            }

            return tables;
        }

        /// <summary>
        /// Recursively extracts table names from subqueries found in SELECT, WHERE, HAVING, and other clauses.
        /// </summary>
        private static HashSet<string> ExtractTablesFromSubqueries(string sql, HashSet<string> cteNames)
        {
            var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(sql))
                return tables;

            // Find all subqueries: (SELECT ...)
            var subqueryPattern = new Regex(
                @"\(\s*SELECT\s+",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var matches = subqueryPattern.Matches(sql);
            foreach (Match match in matches)
            {
                // Extract the complete subquery
                string subquery = ExtractSubquery(sql, match.Index);
                if (!string.IsNullOrWhiteSpace(subquery))
                {
                    // Remove outer parentheses
                    if (subquery.StartsWith("(") && subquery.EndsWith(")"))
                    {
                        subquery = subquery.Substring(1, subquery.Length - 2).Trim();
                    }
                    
                    // Recursively extract tables from subquery
                    var subqueryTables = ExtractTablesFromSelect(subquery);
                    foreach (var table in subqueryTables)
                    {
                        if (!IsCteName(table, cteNames))
                            tables.Add(table);
                    }
                }
            }

            return tables;
        }

        /// <summary>
        /// Extracts a complete subquery starting from the opening parenthesis at the given index.
        /// </summary>
        private static string ExtractSubquery(string sql, int startIndex)
        {
            if (startIndex < 0 || startIndex >= sql.Length)
                return string.Empty;

            int depth = 0;
            bool inString = false;
            char stringChar = '\0';
            int i = startIndex;

            while (i < sql.Length)
            {
                char c = sql[i];
                char next = i + 1 < sql.Length ? sql[i + 1] : '\0';

                // Handle string literals
                if (!inString && (c == '\'' || c == '"'))
                {
                    inString = true;
                    stringChar = c;
                }
                else if (inString && c == stringChar)
                {
                    if (c == '\'' && next == '\'')
                    {
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inString = false;
                    }
                }
                else if (!inString)
                {
                    if (c == '(')
                        depth++;
                    else if (c == ')')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            // Found matching closing parenthesis
                            return sql.Substring(startIndex, i - startIndex + 1);
                        }
                    }
                }

                i++;
            }

            return string.Empty;
        }

        /// <summary>
        /// Splits a SQL statement containing UNION/UNION ALL into individual SELECT statements.
        /// </summary>
        private static List<string> SplitUnionStatements(string sql)
        {
            var parts = new List<string>();
            
            // Pattern to match UNION or UNION ALL (case insensitive)
            var unionPattern = new Regex(
                @"\bUNION\s+(?:ALL\s+)?",
                RegexOptions.IgnoreCase
            );

            var matches = unionPattern.Matches(sql);
            if (matches.Count == 0)
            {
                parts.Add(sql);
                return parts;
            }

            int lastIndex = 0;
            foreach (Match match in matches)
            {
                if (match.Index > lastIndex)
                {
                    parts.Add(sql.Substring(lastIndex, match.Index - lastIndex).Trim());
                }
                lastIndex = match.Index + match.Length;
            }
            
            // Add the last part
            if (lastIndex < sql.Length)
            {
                parts.Add(sql.Substring(lastIndex).Trim());
            }

            return parts;
        }

        /// <summary>
        /// Extracts CTE (Common Table Expression) names from WITH clauses.
        /// </summary>
        private static HashSet<string> ExtractCteNames(string sql)
        {
            var cteNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Pattern to match WITH clause: WITH cte_name AS (...)
            var withPattern = new Regex(
                @"\bWITH\s+(\w+)\s+AS\s*\(",
                RegexOptions.IgnoreCase | RegexOptions.Multiline
            );

            var matches = withPattern.Matches(sql);
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    cteNames.Add(match.Groups[1].Value.Trim());
                }
            }

            // Handle multiple CTEs: WITH cte1 AS (...), cte2 AS (...)
            var multiCtePattern = new Regex(
                @"\bWITH\s+((?:\w+\s+AS\s*\([^)]+\),?\s*)+)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            var multiMatch = multiCtePattern.Match(sql);
            if (multiMatch.Success && multiMatch.Groups.Count > 1)
            {
                string cteList = multiMatch.Groups[1].Value;
                var cteNamePattern = new Regex(@"(\w+)\s+AS\s*\(", RegexOptions.IgnoreCase);
                var cteMatches = cteNamePattern.Matches(cteList);
                foreach (Match cteMatch in cteMatches)
                {
                    if (cteMatch.Groups.Count > 1)
                    {
                        cteNames.Add(cteMatch.Groups[1].Value.Trim());
                    }
                }
            }

            return cteNames;
        }

        /// <summary>
        /// Removes the WITH clause from a SELECT statement.
        /// </summary>
        private static string RemoveCteClause(string sql)
        {
            // Find and remove WITH clause
            var withPattern = new Regex(
                @"\bWITH\s+\w+\s+AS\s*\([^)]+\)\s*,",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            
            sql = withPattern.Replace(sql, string.Empty);
            
            // Handle the last CTE (no trailing comma)
            var lastCtePattern = new Regex(
                @"\bWITH\s+\w+\s+AS\s*\([^)]+\)\s+SELECT",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
            
            sql = lastCtePattern.Replace(sql, "SELECT");
            
            return sql.Trim();
        }

        /// <summary>
        /// Extracts table names from the FROM clause.
        /// </summary>
        private static HashSet<string> ExtractTablesFromFromClause(string sql)
        {
            var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Find FROM clause
            var fromMatch = Regex.Match(sql, @"\bFROM\s+", RegexOptions.IgnoreCase);
            if (!fromMatch.Success)
                return tables;

            int fromIndex = fromMatch.Index + fromMatch.Length;
            
            // Extract everything after FROM until WHERE, GROUP BY, ORDER BY, HAVING, or end
            var endPattern = new Regex(
                @"\b(WHERE|GROUP\s+BY|ORDER\s+BY|HAVING|UNION|$)",
                RegexOptions.IgnoreCase
            );

            var endMatch = endPattern.Match(sql, fromIndex);
            int endIndex = endMatch.Success ? endMatch.Index : sql.Length;
            
            string fromClause = sql.Substring(fromIndex, endIndex - fromIndex).Trim();
            
            // Parse table names from FROM clause (handles joins, subqueries, etc.)
            tables.UnionWith(ParseTableNamesFromClause(fromClause));
            
            return tables;
        }

        /// <summary>
        /// Extracts table names from JOIN clauses.
        /// </summary>
        private static HashSet<string> ExtractTablesFromJoinClause(string sql)
        {
            var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Pattern to match various JOIN types
            var joinPattern = new Regex(
                @"\b(?:INNER\s+|LEFT\s+|RIGHT\s+|FULL\s+)?(?:OUTER\s+)?JOIN\s+",
                RegexOptions.IgnoreCase
            );

            var matches = joinPattern.Matches(sql);
            foreach (Match match in matches)
            {
                int joinIndex = match.Index + match.Length;
                
                // Extract table name after JOIN until ON, WHERE, or next JOIN
                var endPattern = new Regex(
                    @"\b(ON|WHERE|GROUP\s+BY|ORDER\s+BY|HAVING|(?:INNER\s+|LEFT\s+|RIGHT\s+|FULL\s+)?(?:OUTER\s+)?JOIN|$)",
                    RegexOptions.IgnoreCase
                );

                var endMatch = endPattern.Match(sql, joinIndex);
                int endIndex = endMatch.Success ? endMatch.Index : sql.Length;
                
                string joinClause = sql.Substring(joinIndex, endIndex - joinIndex).Trim();
                
                // Parse table name (may include alias)
                tables.UnionWith(ParseTableNamesFromClause(joinClause));
            }

            return tables;
        }

        /// <summary>
        /// Parses table names from a clause (FROM or JOIN), handling aliases, subqueries, and schema qualification.
        /// </summary>
        private static HashSet<string> ParseTableNamesFromClause(string clause)
        {
            var tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(clause))
                return tables;

            // Remove leading/trailing whitespace
            clause = clause.Trim();
            
            // Skip if it's a subquery (starts with '(')
            if (clause.StartsWith("("))
                return tables;

            // Handle comma-separated table list
            var parts = SplitTableList(clause);
            
            foreach (var part in parts)
            {
                string tablePart = part.Trim();
                
                // Skip empty parts
                if (string.IsNullOrWhiteSpace(tablePart))
                    continue;

                // Skip if it's a subquery
                if (tablePart.StartsWith("("))
                    continue;

                // Extract table name (handle schema.table, table@dblink, table alias)
                string tableName = ExtractTableNameFromPart(tablePart);
                
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    tables.Add(tableName);
                }
            }

            return tables;
        }

        /// <summary>
        /// Splits a table list by commas, respecting parentheses and string literals.
        /// </summary>
        private static List<string> SplitTableList(string clause)
        {
            var parts = new List<string>();
            
            if (string.IsNullOrWhiteSpace(clause))
                return parts;

            int depth = 0;
            bool inString = false;
            char stringChar = '\0';
            int start = 0;

            for (int i = 0; i < clause.Length; i++)
            {
                char c = clause[i];
                char next = i + 1 < clause.Length ? clause[i + 1] : '\0';

                // Handle string literals
                if (!inString && (c == '\'' || c == '"'))
                {
                    inString = true;
                    stringChar = c;
                }
                else if (inString && c == stringChar)
                {
                    if (c == '\'' && next == '\'')
                    {
                        i++; // Skip escaped quote
                    }
                    else
                    {
                        inString = false;
                    }
                }
                else if (!inString)
                {
                    if (c == '(')
                        depth++;
                    else if (c == ')')
                        depth--;
                    else if (depth == 0 && c == ',')
                    {
                        parts.Add(clause.Substring(start, i - start).Trim());
                        start = i + 1;
                    }
                }
            }

            // Add the last part
            if (start < clause.Length)
            {
                parts.Add(clause.Substring(start).Trim());
            }

            return parts;
        }

        /// <summary>
        /// Extracts table name from a table reference part (handles schema.table, table@dblink, table alias).
        /// </summary>
        private static string ExtractTableNameFromPart(string part)
        {
            if (string.IsNullOrWhiteSpace(part))
                return string.Empty;

            part = part.Trim();
            
            // Remove alias (everything after AS or space before a word that's not a keyword)
            // Pattern: table_name AS alias or table_name alias
            var aliasPattern = new Regex(
                @"\b(AS|ON|USING|WHERE|GROUP|ORDER|HAVING|INNER|LEFT|RIGHT|FULL|OUTER|JOIN|UNION)\b",
                RegexOptions.IgnoreCase
            );

            var aliasMatch = aliasPattern.Match(part);
            if (aliasMatch.Success)
            {
                part = part.Substring(0, aliasMatch.Index).Trim();
            }
            else
            {
                // Try to find alias by looking for space followed by identifier
                // Simple heuristic: if there's a space and next word looks like alias, remove it
                var spaceMatch = Regex.Match(part, @"\s+(\w+)$");
                if (spaceMatch.Success)
                {
                    // Check if it's likely an alias (not a keyword)
                    string potentialAlias = spaceMatch.Groups[1].Value;
                    if (!IsSqlKeyword(potentialAlias))
                    {
                        part = part.Substring(0, spaceMatch.Index).Trim();
                    }
                }
            }

            // Extract table name (schema.table@dblink or schema.table or table)
            // Remove quotes if present
            part = part.Trim('"', '\'');
            
            // Handle schema.table@dblink format
            var dblinkMatch = Regex.Match(part, @"^(.+?)@");
            if (dblinkMatch.Success)
            {
                part = dblinkMatch.Groups[1].Value;
            }

            // Handle schema.table format - we want the full qualified name
            // So we keep schema.table as is
            
            return part.Trim();
        }

        /// <summary>
        /// Checks if a string is a SQL keyword.
        /// </summary>
        private static bool IsSqlKeyword(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;

            var keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "SELECT", "FROM", "WHERE", "GROUP", "BY", "ORDER", "HAVING",
                "INNER", "LEFT", "RIGHT", "FULL", "OUTER", "JOIN", "ON", "USING",
                "UNION", "ALL", "AS", "AND", "OR", "NOT", "IN", "EXISTS",
                "INSERT", "UPDATE", "DELETE", "CREATE", "DROP", "ALTER",
                "WITH", "CASE", "WHEN", "THEN", "ELSE", "END"
            };

            return keywords.Contains(word);
        }

        /// <summary>
        /// Checks if a name is a CTE name (should be excluded from results).
        /// </summary>
        private static bool IsCteName(string name, HashSet<string> cteNames)
        {
            if (cteNames == null || cteNames.Count == 0)
                return false;

            // Check exact match (case-insensitive)
            if (cteNames.Contains(name))
                return true;

            // Check if name starts with CTE name (for schema-qualified names)
            foreach (var cteName in cteNames)
            {
                if (name.Equals(cteName, StringComparison.OrdinalIgnoreCase) ||
                    name.StartsWith(cteName + ".", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Normalizes table name (handles schema qualification, removes quotes, etc.).
        /// </summary>
        private static string NormalizeTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return string.Empty;

            // Remove surrounding quotes
            tableName = tableName.Trim().Trim('"', '\'');
            
            // Trim whitespace
            tableName = tableName.Trim();
            
            return tableName;
        }

        /// <summary>
        /// Removes SQL comments from the input string.
        /// </summary>
        private static string RemoveComments(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return sql;

            var result = new StringBuilder(sql.Length);
            bool inSingleLineComment = false;
            bool inMultiLineComment = false;
            bool inString = false;
            char stringChar = '\0';

            for (int i = 0; i < sql.Length; i++)
            {
                char c = sql[i];
                char next = i + 1 < sql.Length ? sql[i + 1] : '\0';
                char prev = i > 0 ? sql[i - 1] : '\0';

                // Handle string literals
                if (!inSingleLineComment && !inMultiLineComment)
                {
                    if (!inString && (c == '\'' || c == '"'))
                    {
                        inString = true;
                        stringChar = c;
                        result.Append(c);
                        continue;
                    }
                    else if (inString && c == stringChar)
                    {
                        // Check for escaped quotes
                        if (c == '\'' && next == '\'')
                        {
                            result.Append(c);
                            result.Append(next);
                            i++; // Skip next quote
                            continue;
                        }
                        else if (c == '"' && next == '"')
                        {
                            result.Append(c);
                            result.Append(next);
                            i++; // Skip next quote
                            continue;
                        }
                        else
                        {
                            inString = false;
                            result.Append(c);
                            continue;
                        }
                    }
                    else if (inString)
                    {
                        result.Append(c);
                        continue;
                    }
                }

                // Handle single-line comments (--)
                if (!inString && !inMultiLineComment && c == '-' && next == '-')
                {
                    inSingleLineComment = true;
                    i++; // Skip next dash
                    continue;
                }
                else if (inSingleLineComment && (c == '\n' || c == '\r'))
                {
                    inSingleLineComment = false;
                    if (c == '\r' && next == '\n')
                    {
                        i++; // Skip \n after \r
                    }
                    result.Append(' '); // Replace comment with space
                    continue;
                }
                else if (inSingleLineComment)
                {
                    continue; // Skip comment characters
                }

                // Handle multi-line comments (/* */)
                if (!inString && !inSingleLineComment && c == '/' && next == '*')
                {
                    inMultiLineComment = true;
                    i++; // Skip *
                    continue;
                }
                else if (inMultiLineComment && c == '*' && next == '/')
                {
                    inMultiLineComment = false;
                    i++; // Skip /
                    result.Append(' '); // Replace comment with space
                    continue;
                }
                else if (inMultiLineComment)
                {
                    continue; // Skip comment characters
                }

                result.Append(c);
            }

            return result.ToString();
        }
    }
}
