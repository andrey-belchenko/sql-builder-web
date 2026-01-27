using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Antlr4-based PL/SQL parser for extracting database table names.
    /// Uses proper grammar parsing instead of regex-based approach.
    /// </summary>
    internal static class DevSqlParserAntlr
    {
        /// <summary>
        /// Extracts all database table names from PL/SQL code using Antlr4 parser.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <returns>HashSet of table names (schema-qualified names are preserved)</returns>
        public static HashSet<string> GetSourceTables(string plsqlText)
        {
            // #region agent log
            try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:21\",\"message\":\"GetSourceTables entry\",\"data\":{{\"plsqlLength\":{plsqlText?.Length ?? 0}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"D\"}}\n"); } catch { }
            // #endregion

            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                // Create case-insensitive character stream (PL/SQL grammar is case-sensitive but SQL is case-insensitive)
                var input = new AntlrInputStream(plsqlText);
                var caseChangingStream = new CaseChangingCharStream(input, true); // Convert to uppercase

                // Create lexer and parser
                var lexer = new PlSqlLexer(caseChangingStream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new PlSqlParser(tokens);

                // Remove default error listeners and add a non-throwing one
                // This allows us to extract table names even if there are parse errors
                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ConsoleErrorListener());

                // Parse the input (may produce partial parse tree on errors)
                var tree = parser.sql_script();

                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:48\",\"message\":\"After parsing\",\"data\":{{\"treeIsNull\":{((tree == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"D\"}}\n"); } catch { }
                // #endregion

                // Check if we got a valid parse tree
                if (tree == null)
                {
                    return result;
                }

                // Create visitor to extract table names
                var visitor = new TableNameExtractorVisitor();
                visitor.Visit(tree);

                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:58\",\"message\":\"After visitor\",\"data\":{{\"tableCount\":{visitor.TableNames.Count}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                // #endregion

                // Get results
                foreach (var tableName in visitor.TableNames)
                {
                    if (!string.IsNullOrWhiteSpace(tableName))
                    {
                        result.Add(NormalizeTableName(tableName));
                    }
                }
            }
            catch (Exception ex)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:66\",\"message\":\"Exception caught\",\"data\":{{\"message\":\"{ex.Message?.Replace("\"", "\\\"")}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"D\"}}\n"); } catch { }
                // #endregion
                // If parsing fails, return empty set (graceful degradation)
                // Log the error for debugging
                System.Diagnostics.Debug.WriteLine($"PL/SQL parsing error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return result;
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
        /// Error listener that logs errors but doesn't throw exceptions.
        /// This allows partial parsing to extract table names even when there are syntax errors.
        /// </summary>
        private class ConsoleErrorListener : Antlr4.Runtime.BaseErrorListener
        {
            public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                // Log the error but don't throw - allows partial parsing
                System.Diagnostics.Debug.WriteLine($"PL/SQL parse warning at line {line}, position {charPositionInLine}: {msg}");
            }
        }

        /// <summary>
        /// Custom visitor to extract table names from the parse tree.
        /// </summary>
        private class TableNameExtractorVisitor : PlSqlParserBaseVisitor<object>
        {
            private readonly HashSet<string> _tableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly HashSet<string> _cteNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            public HashSet<string> TableNames => _tableNames;

            // Visit SQL script (entry point)
            public override object VisitSql_script(PlSqlParser.Sql_scriptContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:118\",\"message\":\"VisitSql_script entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                // Visit all unit statements (procedures, packages, anonymous blocks, etc.)
                // The grammar structure shows unit_statement can contain SELECT statements
                return base.VisitSql_script(context);
            }

            // Visit unit statements that may contain SELECT statements
            public override object VisitAnonymous_block(PlSqlParser.Anonymous_blockContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:128\",\"message\":\"VisitAnonymous_block entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;
                
                // Visit seq_of_statements which contains the actual DML statements
                var seqOfStatements = context.seq_of_statements();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:134\",\"message\":\"seqOfStatements check\",\"data\":{{\"seqOfStatementsIsNull\":{((seqOfStatements == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }
                
                // Don't call base.VisitAnonymous_block to avoid double-visiting
                return null;
            }

            // Visit sequence of statements
            public override object VisitSeq_of_statements(PlSqlParser.Seq_of_statementsContext context)
            {
                if (context == null) return null;
                
                // Visit all statement items
                foreach (var statement in context.statement())
                {
                    Visit(statement);
                }
                
                // Don't call base.VisitSeq_of_statements to avoid double-visiting
                return null;
            }

            // Visit statement (can contain sql_statement, block, etc.)
            public override object VisitStatement(PlSqlParser.StatementContext context)
            {
                if (context == null) return null;

                // Visit sql_statement which contains DML statements
                var sqlStmt = context.sql_statement();
                if (sqlStmt != null)
                {
                    Visit(sqlStmt);
                }

                // Don't call base.VisitStatement to avoid double-visiting
                return null;
            }

            // Visit SQL statement (contains data_manipulation_language_statements)
            public override object VisitSql_statement(PlSqlParser.Sql_statementContext context)
            {
                if (context == null) return null;

                // Visit data_manipulation_language_statements which contains SELECT, INSERT, UPDATE, DELETE
                var dmlStatements = context.data_manipulation_language_statements();
                if (dmlStatements != null)
                {
                    Visit(dmlStatements);
                }

                // Don't call base.VisitSql_statement to avoid double-visiting
                return null;
            }

            // Visit data manipulation language statements (SELECT, INSERT, UPDATE, DELETE, MERGE)
            public override object VisitData_manipulation_language_statements(PlSqlParser.Data_manipulation_language_statementsContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:191\",\"message\":\"VisitData_manipulation_language_statements entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                // Visit SELECT statement
                var selectStmt = context.select_statement();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:196\",\"message\":\"select_statement check\",\"data\":{{\"selectStmtIsNull\":{((selectStmt == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"B\"}}\n"); } catch { }
                // #endregion
                if (selectStmt != null)
                {
                    VisitSelect_statement(selectStmt);
                }

                // Visit INSERT statement
                var insertStmt = context.insert_statement();
                if (insertStmt != null)
                {
                    VisitInsert_statement(insertStmt);
                }

                // Visit UPDATE statement
                var updateStmt = context.update_statement();
                if (updateStmt != null)
                {
                    VisitUpdate_statement(updateStmt);
                }

                // Visit DELETE statement
                var deleteStmt = context.delete_statement();
                if (deleteStmt != null)
                {
                    VisitDelete_statement(deleteStmt);
                }

                // Visit MERGE statement
                var mergeStmt = context.merge_statement();
                if (mergeStmt != null)
                {
                    VisitMerge_statement(mergeStmt);
                }

                // Don't call base.VisitData_manipulation_language_statements to avoid double-visiting
                return null;
            }


            // Visit INSERT statements
            public override object VisitInsert_statement(PlSqlParser.Insert_statementContext context)
            {
                if (context == null) return null;

                var singleInsert = context.single_table_insert();
                if (singleInsert != null)
                {
                    // Visit SELECT statement if present (source tables)
                    // Do NOT extract target table from insert_into_clause
                    var selectStmt = singleInsert.select_statement();
                    if (selectStmt != null)
                    {
                        VisitSelect_statement(selectStmt);
                    }
                }

                var multiInsert = context.multi_table_insert();
                if (multiInsert != null)
                {
                    // Visit SELECT statement (source tables)
                    var selectStmt = multiInsert.select_statement();
                    if (selectStmt != null)
                    {
                        VisitSelect_statement(selectStmt);
                    }

                    // Do NOT extract target tables from multi_table_element
                }

                // Don't call base.VisitInsert_statement to avoid double-visiting
                return null;
            }

            // Override to prevent extracting target table from INSERT INTO clause
            public override object VisitInsert_into_clause(PlSqlParser.Insert_into_clauseContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table name from INSERT INTO clause
                // This is the target table, not a source table
                // Don't visit children to avoid extracting table names
                return null;
            }

            // Visit UPDATE statements
            public override object VisitUpdate_statement(PlSqlParser.Update_statementContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table from UPDATE statement
                // Only visit subqueries in WHERE clauses, SET expressions, etc.
                // Visit WHERE clause if present (may contain subqueries)
                var whereClause = context.where_clause();
                if (whereClause != null)
                {
                    Visit(whereClause);
                }

                // Visit SET clause expressions (may contain subqueries)
                var updateSetClause = context.update_set_clause();
                if (updateSetClause != null)
                {
                    Visit(updateSetClause);
                }

                // Don't call base.VisitUpdate_statement to avoid extracting target table
                return null;
            }

            // Visit DELETE statements
            public override object VisitDelete_statement(PlSqlParser.Delete_statementContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table from DELETE statement
                // Only visit subqueries in WHERE clauses, etc.
                // Visit WHERE clause if present (may contain subqueries)
                var whereClause = context.where_clause();
                if (whereClause != null)
                {
                    Visit(whereClause);
                }

                // Don't call base.VisitDelete_statement to avoid extracting target table
                return null;
            }

            // Override to prevent extracting target table from DELETE/UPDATE FROM clause
            // Table extraction is handled through VisitTable_ref_aux (FROM clauses in SELECT statements)
            // This method is used for target tables in DELETE/UPDATE statements, which we don't want
            public override object VisitGeneral_table_ref(PlSqlParser.General_table_refContext context)
            {
                if (context == null) return null;

                // Do NOT extract table names from general_table_ref
                // This is used for target tables in DELETE/UPDATE statements
                // Source tables are extracted through VisitTable_ref_aux in FROM clauses
                // Don't visit children to avoid extracting target table names
                return null;
            }

            // Visit MERGE statements
            public override object VisitMerge_statement(PlSqlParser.Merge_statementContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table from INTO clause
                // Visit USING clause (contains source tables)
                // selected_tableview: (tableview_name | '(' select_statement ')' | table_collection_expression | '(' table_collection_expression ')') table_alias?
                var selectedTableview = context.selected_tableview();
                if (selectedTableview != null)
                {
                    // Check for tableview_name (source table in USING clause)
                    var tvName = selectedTableview.tableview_name();
                    if (tvName != null)
                    {
                        ExtractTableName(tvName);
                    }

                    // Check for SELECT statement (source in USING clause)
                    var selectStmt = selectedTableview.select_statement();
                    if (selectStmt != null)
                    {
                        VisitSelect_statement(selectStmt);
                    }
                }

                // Don't call base.VisitMerge_statement to avoid double-visiting
                return null;
            }

            // Visit SELECT statements
            public override object VisitSelect_statement(PlSqlParser.Select_statementContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:399\",\"message\":\"VisitSelect_statement entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"B\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                var selectOnly = context.select_only_statement();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:403\",\"message\":\"select_only_statement check\",\"data\":{{\"selectOnlyIsNull\":{((selectOnly == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"B\"}}\n"); } catch { }
                // #endregion
                if (selectOnly != null)
                {
                    // First, collect CTE names from WITH clause
                    var withClause = selectOnly.with_clause();
                    if (withClause != null)
                    {
                        VisitWithClause(withClause);
                    }

                    // Then visit the subquery to extract table names
                    var subquery = selectOnly.subquery();
                    // #region agent log
                    try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:415\",\"message\":\"subquery check\",\"data\":{{\"subqueryIsNull\":{((subquery == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"B\"}}\n"); } catch { }
                    // #endregion
                    if (subquery != null)
                    {
                        VisitSubquery(subquery);
                    }
                }

                // Don't call base.VisitSelect_statement to avoid double-visiting
                return null;
            }

            // Visit WITH clause to collect CTE names and extract source tables from CTE definitions
            private void VisitWithClause(PlSqlParser.With_clauseContext context)
            {
                if (context == null) return;

                // Visit all factoring clauses
                foreach (var factoring in context.with_factoring_clause())
                {
                    var subqueryFactoring = factoring.subquery_factoring_clause();
                    if (subqueryFactoring != null)
                    {
                        // Extract CTE name
                        // query_name is identifier according to grammar
                        var queryName = subqueryFactoring.query_name();
                        if (queryName != null)
                        {
                            var identifier = queryName.identifier();
                            if (identifier != null)
                            {
                                string cteName = GetIdentifierText(identifier);
                                if (!string.IsNullOrWhiteSpace(cteName))
                                {
                                    _cteNames.Add(cteName);
                                }
                            }
                        }

                        // Visit the CTE definition subquery to extract source tables
                        // subquery_factoring_clause: query_name ... AS '(' subquery ... ')'
                        var cteSubquery = subqueryFactoring.subquery();
                        if (cteSubquery != null)
                        {
                            VisitSubquery(cteSubquery);
                        }
                    }
                }
            }

            // Visit subquery to extract table names
            public override object VisitSubquery(PlSqlParser.SubqueryContext context)
            {
                if (context == null) return null;

                // Visit query blocks
                var basicElements = context.subquery_basic_elements();
                if (basicElements != null)
                {
                    var queryBlock = basicElements.query_block();
                    if (queryBlock != null)
                    {
                        Visit(queryBlock);
                    }

                    var nestedSubquery = basicElements.subquery();
                    if (nestedSubquery != null)
                    {
                        VisitSubquery(nestedSubquery);
                    }
                }

                // Visit UNION parts
                foreach (var operationPart in context.subquery_operation_part())
                {
                    var unionBasicElements = operationPart.subquery_basic_elements();
                    if (unionBasicElements != null)
                    {
                        var unionQueryBlock = unionBasicElements.query_block();
                        if (unionQueryBlock != null)
                        {
                            Visit(unionQueryBlock);
                        }

                        var unionSubquery = unionBasicElements.subquery();
                        if (unionSubquery != null)
                        {
                            VisitSubquery(unionSubquery);
                        }
                    }
                }

                // Don't call base.VisitSubquery to avoid double-visiting and infinite loops
                return null;
            }

            // Visit query block to extract table names from FROM and JOIN clauses
            public override object VisitQuery_block(PlSqlParser.Query_blockContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:505\",\"message\":\"VisitQuery_block entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                // Visit FROM clause
                var fromClause = context.from_clause();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:511\",\"message\":\"from_clause check\",\"data\":{{\"fromClauseIsNull\":{((fromClause == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (fromClause != null)
                {
                    Visit(fromClause);
                }

                // Don't call base.VisitQuery_block to avoid double-visiting
                return null;
            }

            // Visit FROM clause
            public override object VisitFrom_clause(PlSqlParser.From_clauseContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:520\",\"message\":\"VisitFrom_clause entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                var tableRefList = context.table_ref_list();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:525\",\"message\":\"table_ref_list check\",\"data\":{{\"tableRefListIsNull\":{((tableRefList == null) ? "true" : "false")},\"tableRefCount\":{(tableRefList?.table_ref()?.Length ?? 0)}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (tableRefList != null)
                {
                    foreach (var tableRef in tableRefList.table_ref())
                    {
                        Visit(tableRef);
                    }
                }

                // Don't call base.VisitFrom_clause to avoid double-visiting
                return null;
            }

            // Visit table reference
            public override object VisitTable_ref(PlSqlParser.Table_refContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:551\",\"message\":\"VisitTable_ref entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                // Visit the main table reference
                var aux = context.table_ref_aux();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:558\",\"message\":\"table_ref_aux check\",\"data\":{{\"auxIsNull\":{((aux == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (aux != null)
                {
                    Visit(aux);
                }

                // Visit JOIN clauses
                foreach (var joinClause in context.join_clause())
                {
                    Visit(joinClause);
                }

                // Don't call base.VisitTable_ref to avoid double-visiting and infinite loops
                return null;
            }

            // Visit table reference aux (main table or subquery)
            public override object VisitTable_ref_aux(PlSqlParser.Table_ref_auxContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:570\",\"message\":\"VisitTable_ref_aux entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                var tableRefInternal = context.table_ref_aux_internal();
                // #region agent log
                try { 
                    string altType = "unknown";
                    if (tableRefInternal != null)
                    {
                        if (tableRefInternal is PlSqlParser.Table_ref_aux_internal_oneContext) altType = "one";
                        else if (tableRefInternal is PlSqlParser.Table_ref_aux_internal_twoContext) altType = "two";
                        else if (tableRefInternal is PlSqlParser.Table_ref_aux_internal_threContext) altType = "thre";
                    }
                    File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:575\",\"message\":\"table_ref_aux_internal check\",\"data\":{{\"tableRefInternalIsNull\":{((tableRefInternal == null) ? "true" : "false")},\"altType\":\"{altType}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\"}}\n"); 
                } catch { }
                // #endregion
                if (tableRefInternal != null)
                {
                    // table_ref_aux_internal is a labeled alternative with three options.
                    // Visit children and let the visitor handle the alternatives recursively.
                    // We'll also check for dml_table_expression_clause and subquery in the children.
                    Visit(tableRefInternal);
                }

                // Also check for table_alias which might contain tableview_name
                // Actually, table_alias is just an alias, not the table name itself
                // Don't call base.VisitTable_ref_aux to avoid double-visiting
                return null;
            }

            // Override visitor methods for labeled alternatives of table_ref_aux_internal
            public override object VisitTable_ref_aux_internal_one(PlSqlParser.Table_ref_aux_internal_oneContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:628\",\"message\":\"VisitTable_ref_aux_internal_one entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                var dmlTableExpr = context.dml_table_expression_clause();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:635\",\"message\":\"dml_table_expression_clause check\",\"data\":{{\"dmlTableExprIsNull\":{((dmlTableExpr == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                // #endregion
                if (dmlTableExpr != null)
                {
                    Visit(dmlTableExpr);
                }
                else
                {
                    // If dml_table_expression_clause is null, visit children to find what's actually there
                    // #region agent log
                    try { 
                        int childCount = context.ChildCount;
                        var childTypes = new List<string>();
                        for (int i = 0; i < childCount; i++)
                        {
                            var child = context.GetChild(i);
                            if (child != null)
                            {
                                childTypes.Add(child.GetType().Name);
                            }
                        }
                        File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:646\",\"message\":\"VisitChildren in table_ref_aux_internal_one\",\"data\":{{\"childCount\":{childCount},\"childTypes\":\"{string.Join(",", childTypes)}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); 
                    } catch { }
                    // #endregion
                    VisitChildren(context);
                }

                // Don't call base.VisitTable_ref_aux_internal_one to avoid double-visiting
                return null;
            }

            public override object VisitTable_ref_aux_internal_two(PlSqlParser.Table_ref_aux_internal_twoContext context)
            {
                if (context == null) return null;

                var tableRef = context.table_ref();
                if (tableRef != null)
                {
                    Visit(tableRef);
                }

                // Don't call base.VisitTable_ref_aux_internal_two to avoid double-visiting
                return null;
            }

            public override object VisitTable_ref_aux_internal_thre(PlSqlParser.Table_ref_aux_internal_threContext context)
            {
                if (context == null) return null;

                var dmlTableExpr = context.dml_table_expression_clause();
                if (dmlTableExpr != null)
                {
                    Visit(dmlTableExpr);
                }

                // Don't call base.VisitTable_ref_aux_internal_thre to avoid double-visiting
                return null;
            }

            // Visit DML table expression clause (contains tableview_name, subquery, or select_statement)
            public override object VisitDml_table_expression_clause(PlSqlParser.Dml_table_expression_clauseContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:682\",\"message\":\"VisitDml_table_expression_clause entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (context == null) return null;

                var tableviewName = context.tableview_name();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:689\",\"message\":\"tableview_name check\",\"data\":{{\"tableviewNameIsNull\":{((tableviewName == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (tableviewName != null)
                {
                    ExtractTableName(tableviewName);
                    // Don't visit children if we found tableview_name directly
                    return null;
                }

                // Check for select_statement (wrapped in parentheses)
                var selectStmt = context.select_statement();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:701\",\"message\":\"select_statement check\",\"data\":{{\"selectStmtIsNull\":{((selectStmt == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (selectStmt != null)
                {
                    VisitSelect_statement(selectStmt);
                    return null;
                }

                // Check for subqueries
                var subquery = context.subquery();
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:709\",\"message\":\"subquery in dml_table_expression_clause check\",\"data\":{{\"subqueryIsNull\":{((subquery == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); } catch { }
                // #endregion
                if (subquery != null)
                {
                    VisitSubquery(subquery);
                    return null;
                }

                // If we get here, tableview_name, select_statement, and subquery are all null
                // Visit children to find what's actually there (might be table_collection_expression or other structures)
                // #region agent log
                try { 
                    int childCount = context.ChildCount;
                    var childTypes = new List<string>();
                    for (int i = 0; i < childCount; i++)
                    {
                        var child = context.GetChild(i);
                        if (child != null)
                        {
                            childTypes.Add(child.GetType().Name);
                        }
                    }
                    File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:721\",\"message\":\"Visiting children fallback\",\"data\":{{\"childCount\":{childCount},\"childTypes\":\"{string.Join(",", childTypes)}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\"}}\n"); 
                } catch { }
                // #endregion
                VisitChildren(context);

                return null;
            }

            // Visit JOIN clause
            public override object VisitJoin_clause(PlSqlParser.Join_clauseContext context)
            {
                if (context == null) return null;

                var tableRefAux = context.table_ref_aux();
                if (tableRefAux != null)
                {
                    Visit(tableRefAux);
                }

                // Don't call base.VisitJoin_clause to avoid double-visiting
                return null;
            }

            // Override VisitTableview_name to catch any tableview_name contexts visited through VisitChildren
            public override object VisitTableview_name(PlSqlParser.Tableview_nameContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:742\",\"message\":\"VisitTableview_name (via VisitChildren)\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"D\"}}\n"); } catch { }
                // #endregion
                if (context != null)
                {
                    ExtractTableName(context);
                }
                return null;
            }

            // Extract table name from tableview_name context
            private void ExtractTableName(PlSqlParser.Tableview_nameContext context)
            {
                // #region agent log
                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:752\",\"message\":\"ExtractTableName entry\",\"data\":{{\"contextIsNull\":{((context == null) ? "true" : "false")}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"B\"}}\n"); } catch { }
                // #endregion
                if (context == null) return;

                // tableview_name: identifier ('.' id_expression)? (AT_SIGN link_name | partition_extension_clause)?
                var identifier = context.identifier();
                if (identifier != null)
                {
                    string schema = GetIdentifierText(identifier);
                    string tableName = schema;

                    // Check for schema qualification (identifier.id_expression)
                    // id_expression is optional single item, not an array
                    var idExpression = context.id_expression();
                    if (idExpression != null)
                    {
                        // Schema-qualified: schema.table
                        string table = GetIdExpressionText(idExpression);
                        if (!string.IsNullOrWhiteSpace(table))
                        {
                            tableName = schema + "." + table;
                        }
                    }

                    // Handle database links (table@dblink)
                    var linkName = context.link_name();
                    string dblink = null;
                    if (linkName != null)
                    {
                        var database = linkName.database();
                        if (database != null)
                        {
                            var dbIdExpr = database.id_expression();
                            if (dbIdExpr != null)
                            {
                                dblink = GetIdExpressionText(dbIdExpr);
                            }
                        }
                    }

                    // #region agent log
                    try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:697\",\"message\":\"Before CTE check\",\"data\":{{\"tableName\":\"{tableName?.Replace("\"", "\\\"")}\",\"cteCount\":{_cteNames.Count}}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                    // #endregion

                    // Check if it's a CTE (should be excluded)
                    string baseTableName = tableName;
                    if (!string.IsNullOrWhiteSpace(baseTableName))
                    {
                        // Check base name and schema-qualified name
                        if (!_cteNames.Contains(baseTableName))
                        {
                            // Extract schema and table separately for CTE check
                            int dotIndex = baseTableName.IndexOf('.');
                            if (dotIndex > 0)
                            {
                                string tablePart = baseTableName.Substring(dotIndex + 1);
                                if (!_cteNames.Contains(tablePart))
                                {
                                    _tableNames.Add(tableName);
                                    // #region agent log
                                    try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:710\",\"message\":\"Added table name (schema qualified)\",\"data\":{{\"tableName\":\"{tableName?.Replace("\"", "\\\"")}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                                    // #endregion
                                }
                            }
                            else
                            {
                                _tableNames.Add(tableName);
                                // #region agent log
                                try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:717\",\"message\":\"Added table name\",\"data\":{{\"tableName\":\"{tableName?.Replace("\"", "\\\"")}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                                // #endregion
                            }
                        }
                        else
                        {
                            // #region agent log
                            try { File.AppendAllText(@"c:\Repos\github\sql-builder-web\net\.cursor\debug.log", $"{{\"id\":\"log_{DateTime.UtcNow.Ticks}\",\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()},\"location\":\"DevSqlParserAntlr.cs:723\",\"message\":\"Skipped table (CTE)\",\"data\":{{\"tableName\":\"{tableName?.Replace("\"", "\\\"")}\"}},\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\"}}\n"); } catch { }
                            // #endregion
                        }
                    }
                }
            }

            // Helper to get text from identifier
            private string GetIdentifierText(PlSqlParser.IdentifierContext context)
            {
                if (context == null) return string.Empty;

                var idExpr = context.id_expression();
                if (idExpr != null)
                {
                    return GetIdExpressionText(idExpr);
                }

                return string.Empty;
            }

            // Helper to get text from id_expression
            private string GetIdExpressionText(PlSqlParser.Id_expressionContext context)
            {
                if (context == null) return string.Empty;

                // id_expression: regular_id | DELIMITED_ID
                var delimitedId = context.DELIMITED_ID();
                if (delimitedId != null)
                {
                    string text = delimitedId.GetText();
                    // Remove surrounding quotes and unescape
                    if (text.Length >= 2 && text.StartsWith("\"") && text.EndsWith("\""))
                    {
                        text = text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
                    }
                    return text;
                }

                var regularId = context.regular_id();
                if (regularId != null)
                {
                    // regular_id can be REGULAR_ID token or various keywords
                    var regularIdToken = regularId.REGULAR_ID();
                    if (regularIdToken != null)
                    {
                        return regularIdToken.GetText();
                    }
                    // If it's a keyword, get the text from the context
                    return regularId.GetText();
                }

                return string.Empty;
            }
        }
    }
}
