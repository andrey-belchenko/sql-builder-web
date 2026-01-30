using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
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
        /// Extracts the SELECT statement from CREATE MATERIALIZED VIEW DDL.
        /// Returns null if the input is not a materialized view DDL.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string</param>
        /// <returns>SELECT statement text, or null if not a materialized view</returns>
        private static string ExtractSelectFromMatView(string plsqlText)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return null;

            string trimmed = plsqlText.Trim();
            
            // Check if it starts with CREATE MATERIALIZED VIEW (case-insensitive)
            if (!trimmed.StartsWith("CREATE", StringComparison.OrdinalIgnoreCase))
                return null;

            // Find "MATERIALIZED VIEW" (case-insensitive)
            int createPos = 0;
            int materializedPos = trimmed.IndexOf("MATERIALIZED", createPos, StringComparison.OrdinalIgnoreCase);
            if (materializedPos < 0)
                return null;

            int viewPos = trimmed.IndexOf("VIEW", materializedPos + "MATERIALIZED".Length, StringComparison.OrdinalIgnoreCase);
            if (viewPos < 0)
                return null;

            // Find "AS" keyword after the VIEW keyword (case-insensitive)
            // We need to find the AS that precedes the SELECT statement
            // This is tricky because AS can appear in other contexts (e.g., column aliases)
            // We'll look for "AS" followed by "SELECT" or "WITH"
            int searchStart = viewPos + "VIEW".Length;
            int asPos = -1;
            
            while (true)
            {
                asPos = trimmed.IndexOf("AS", searchStart, StringComparison.OrdinalIgnoreCase);
                if (asPos < 0)
                    return null;

                // Check what comes after "AS" - skip whitespace and check for SELECT or WITH
                int afterAs = asPos + "AS".Length;
                int nextNonWhitespace = afterAs;
                while (nextNonWhitespace < trimmed.Length && char.IsWhiteSpace(trimmed[nextNonWhitespace]))
                {
                    nextNonWhitespace++;
                }

                if (nextNonWhitespace >= trimmed.Length)
                {
                    searchStart = afterAs;
                    continue;
                }

                // Check if next token is SELECT or WITH
                string remaining = trimmed.Substring(nextNonWhitespace);
                if (remaining.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) ||
                    remaining.StartsWith("WITH", StringComparison.OrdinalIgnoreCase))
                {
                    // Found the AS that precedes the SELECT statement
                    break;
                }

                // This AS is not the one we want, continue searching
                searchStart = afterAs;
            }

            if (asPos < 0)
                return null;

            // Extract everything after "AS" (the SELECT statement)
            int selectStart = asPos + "AS".Length;
            while (selectStart < trimmed.Length && char.IsWhiteSpace(trimmed[selectStart]))
            {
                selectStart++;
            }

            if (selectStart >= trimmed.Length)
                return null;

            return trimmed.Substring(selectStart).Trim();
        }

        /// <summary>
        /// Extracts all database table names from PL/SQL code using Antlr4 parser.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <param name="procedureName">Optional procedure or function name. If specified, only extracts tables from that procedure/function.</param>
        /// <returns>HashSet of table names (schema-qualified names are preserved)</returns>
        public static HashSet<string> GetSourceTables(string plsqlText, string procedureName = null)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Check if this is a materialized view DDL and extract SELECT statement
            // If it's a mat view, use the extracted SELECT; otherwise use original text
            string textToParse = ExtractSelectFromMatView(plsqlText) ?? plsqlText;

            // Normalize procedure name for comparison (case-insensitive)
            string normalizedProcedureName = null;
            if (!string.IsNullOrWhiteSpace(procedureName))
            {
                normalizedProcedureName = procedureName.Trim().ToUpperInvariant();
            }

            // Create case-insensitive character stream (PL/SQL grammar is case-sensitive but SQL is case-insensitive)
            var input = new AntlrInputStream(textToParse);
            var caseChangingStream = new CaseChangingCharStream(input, true); // Convert to uppercase

            // Create lexer and parser
            var lexer = new PlSqlLexer(caseChangingStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new PlSqlParser(tokens);

            // Check if input looks like a standalone SELECT statement (starts with SELECT, no CREATE/BEGIN/DECLARE)
            string trimmedText = textToParse.Trim();
            bool looksLikeStandaloneSelect = trimmedText.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) ||
                                             trimmedText.StartsWith("WITH", StringComparison.OrdinalIgnoreCase);
            
            // Remove default error listeners and add a throwing error listener
            var errorListener = new ThrowingErrorListener(textToParse);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            IParseTree tree = null;
            
            // Create visitor to extract table names (reused for both paths)
            var visitor = new TableNameExtractorVisitor(normalizedProcedureName);
            
            // If it looks like a standalone SELECT, try parsing as select_statement first
            if (looksLikeStandaloneSelect)
            {
                try
                {
                    tree = parser.select_statement();
                    if (tree != null && !errorListener.HasErrors)
                    {
                        // Successfully parsed as standalone SELECT
                        visitor.VisitSelect_statement((PlSqlParser.Select_statementContext)tree);
                        
                        // Get results
                        foreach (var tableName in visitor.TableNames)
                        {
                            if (!string.IsNullOrWhiteSpace(tableName))
                            {
                                var normalized = NormalizeTableName(tableName);
                                result.Add(normalized);
                            }
                        }
                        
                        return result;
                    }
                }
                catch
                {
                    // If standalone SELECT parsing fails, try sql_script instead
                    tokens.Seek(0);
                    lexer.Reset();
                    parser.Reset();
                    errorListener.ClearErrors();
                }
            }

            // Parse as sql_script (for full PL/SQL scripts or if standalone SELECT failed)
            tree = parser.sql_script();

            // Check if we got a valid parse tree
            if (tree == null)
            {
                errorListener.ThrowIfErrors();
                return result;
            }

            // Throw if any parsing errors occurred
            errorListener.ThrowIfErrors();

            // Use visitor to extract table names
            visitor.Visit(tree);

            // Get results
            foreach (var tableName in visitor.TableNames)
            {
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    var normalized = NormalizeTableName(tableName);
                    result.Add(normalized);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts all stored procedure calls from PL/SQL code using Antlr4 parser.
        /// </summary>
        /// <param name="plsqlText">PL/SQL code string (can include anonymous blocks, procedures, packages, etc.)</param>
        /// <param name="procedureName">Optional procedure or function name. If specified, only extracts procedures from that procedure/function.</param>
        /// <returns>HashSet of procedure names (package-qualified names are preserved, same-package calls are resolved)</returns>
        public static HashSet<string> GetSourceProcedures(string plsqlText, string procedureName = null)
        {
            if (string.IsNullOrWhiteSpace(plsqlText))
                return new HashSet<string>();

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Check if this is a materialized view DDL and extract SELECT statement
            // If it's a mat view, use the extracted SELECT; otherwise use original text
            string textToParse = ExtractSelectFromMatView(plsqlText) ?? plsqlText;

            // Create case-insensitive character stream (PL/SQL grammar is case-sensitive but SQL is case-insensitive)
            var input = new AntlrInputStream(textToParse);
            var caseChangingStream = new CaseChangingCharStream(input, true); // Convert to uppercase

            // Create lexer and parser
            var lexer = new PlSqlLexer(caseChangingStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new PlSqlParser(tokens);

            // Remove default error listeners and add a throwing error listener
            var errorListener = new ThrowingErrorListener(textToParse);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            // Parse the input
            var tree = parser.sql_script();

            // Check if we got a valid parse tree
            if (tree == null)
            {
                errorListener.ThrowIfErrors();
                return result;
            }

            // Throw if any parsing errors occurred
            errorListener.ThrowIfErrors();

            // Normalize procedure name for comparison (case-insensitive)
            string normalizedProcedureName = null;
            if (!string.IsNullOrWhiteSpace(procedureName))
            {
                normalizedProcedureName = procedureName.Trim().ToUpperInvariant();
            }

            // Create visitor to extract procedure names
            var visitor = new ProcedureCallExtractorVisitor(normalizedProcedureName);
            visitor.Visit(tree);

            // Get results
            foreach (var procName in visitor.ProcedureNames)
            {
                if (!string.IsNullOrWhiteSpace(procName))
                {
                    var normalized = NormalizeProcedureName(procName);
                    result.Add(normalized);
                }
            }

            return result;
        }

        /// <summary>
        /// Normalizes procedure name (handles schema qualification, removes quotes, etc.).
        /// </summary>
        private static string NormalizeProcedureName(string procedureName)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
                return string.Empty;

            // Remove surrounding quotes
            procedureName = procedureName.Trim().Trim('"', '\'');
            
            // Trim whitespace
            procedureName = procedureName.Trim();
            
            return procedureName;
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
        /// Gets the project root directory (where SqlBuilder.slnx is located).
        /// </summary>
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
        /// Error listener that collects parsing errors and throws exceptions when parsing fails.
        /// Implements both lexer (int) and parser (IToken) error listener interfaces.
        /// </summary>
        private class ThrowingErrorListener : Antlr4.Runtime.BaseErrorListener, Antlr4.Runtime.IAntlrErrorListener<int>
        {
            private readonly List<string> _errors = new List<string>();
            private readonly string _plsqlText;

            public bool HasErrors => _errors.Count > 0;

            public IReadOnlyList<string> Errors => _errors;

            public ThrowingErrorListener(string plsqlText = null)
            {
                _plsqlText = plsqlText;
            }

            // Parser error handler (IToken)
            public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                string errorMessage = $"line {line}:{charPositionInLine} {msg}";
                if (offendingSymbol != null)
                {
                    errorMessage += $" at: '{offendingSymbol.Text}'";
                }
                _errors.Add(errorMessage);
            }

            // Lexer error handler (int)
            void Antlr4.Runtime.IAntlrErrorListener<int>.SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                string errorMessage = $"line {line}:{charPositionInLine} {msg}";
                if (offendingSymbol >= 0)
                {
                    // Try to get the text from the recognizer if possible
                    if (recognizer is Antlr4.Runtime.Lexer lexer)
                    {
                        var vocab = lexer.Vocabulary;
                        if (vocab != null)
                        {
                            string symbolName = vocab.GetSymbolicName(offendingSymbol) ?? vocab.GetLiteralName(offendingSymbol);
                            if (!string.IsNullOrEmpty(symbolName))
                            {
                                errorMessage += $" at: {symbolName}";
                            }
                            else
                            {
                                errorMessage += $" at: '{offendingSymbol}'";
                            }
                        }
                        else
                        {
                            errorMessage += $" at: '{offendingSymbol}'";
                        }
                    }
                    else
                    {
                        errorMessage += $" at: '{offendingSymbol}'";
                    }
                }
                _errors.Add(errorMessage);
            }

            public void ClearErrors()
            {
                _errors.Clear();
            }

            public void ThrowIfErrors()
            {
                if (_errors.Count > 0)
                {
                    // Save SQL text to Temp folder if available
                    if (!string.IsNullOrWhiteSpace(_plsqlText))
                    {
                        try
                        {
                            // Get project root directory (where SqlBuilder.slnx is located)
                            string projectRoot = DevSqlParserAntlr.GetProjectRoot();
                            if (!string.IsNullOrEmpty(projectRoot))
                            {
                                // Ensure Temp folder exists
                                string tempFolder = Path.Combine(projectRoot, "Temp");
                                Directory.CreateDirectory(tempFolder);

                                string fileName = $"plsql_error_{DateTime.Now:yyyyMMdd_HHmmss_fff}.sql";
                                string filePath = Path.Combine(tempFolder, fileName);
                                File.WriteAllText(filePath, _plsqlText, Encoding.UTF8);
                            }
                        }
                        catch
                        {
                            // Ignore errors when saving file - don't prevent exception from being thrown
                        }
                    }

                    string combinedMessage = string.Join(Environment.NewLine, _errors);
                    throw new InvalidOperationException($"PL/SQL parsing failed with {_errors.Count} error(s):{Environment.NewLine}{combinedMessage}");
                }
            }
        }

        /// <summary>
        /// Custom visitor to extract table names from the parse tree.
        /// </summary>
        private class TableNameExtractorVisitor : PlSqlParserBaseVisitor<object>
        {
            private readonly HashSet<string> _tableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly HashSet<string> _cteNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly string _targetProcedureName;
            private string _currentProcedureName;

            public HashSet<string> TableNames => _tableNames;

            public TableNameExtractorVisitor(string targetProcedureName = null)
            {
                _targetProcedureName = targetProcedureName;
            }

            /// <summary>
            /// Checks if we should extract tables in the current context.
            /// Returns true if no target procedure is specified, or if we're inside the target procedure.
            /// </summary>
            private bool ShouldExtractTables()
            {
                return string.IsNullOrWhiteSpace(_targetProcedureName) || 
                       (string.Equals(_currentProcedureName, _targetProcedureName, StringComparison.OrdinalIgnoreCase));
            }

            // Visit SQL script (entry point)
            public override object VisitSql_script(PlSqlParser.Sql_scriptContext context)
            {
                if (context == null) return null;

                // Visit all unit statements (procedures, packages, anonymous blocks, etc.)
                // The grammar structure shows unit_statement can contain SELECT statements
                return base.VisitSql_script(context);
            }

            // Visit unit statements that may contain SELECT statements
            public override object VisitAnonymous_block(PlSqlParser.Anonymous_blockContext context)
            {
                if (context == null) return null;
                
                // Visit seq_of_statements which contains the actual DML statements
                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }
                
                // Don't call base.VisitAnonymous_block to avoid double-visiting
                return null;
            }

            // Visit CREATE PACKAGE BODY statements
            public override object VisitCreate_package_body(PlSqlParser.Create_package_bodyContext context)
            {
                if (context == null) return null;

                // Visit all package_obj_body elements (procedures, functions, etc.)
                foreach (var packageObjBody in context.package_obj_body())
                {
                    if (packageObjBody != null)
                    {
                        Visit(packageObjBody);
                    }
                }

                // Visit optional BEGIN seq_of_statements section (package initialization)
                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }

                // Don't call base.VisitCreate_package_body to avoid double-visiting
                return null;
            }

            // Visit package object body (can be procedure_body, function_body, etc.)
            public override object VisitPackage_obj_body(PlSqlParser.Package_obj_bodyContext context)
            {
                if (context == null) return null;

                // Visit procedure_body if present
                var procedureBody = context.procedure_body();
                if (procedureBody != null)
                {
                    Visit(procedureBody);
                }

                // Visit function_body if present
                var functionBody = context.function_body();
                if (functionBody != null)
                {
                    Visit(functionBody);
                }

                // Don't call base.VisitPackage_obj_body to avoid double-visiting
                return null;
            }

            // Visit procedure body
            public override object VisitProcedure_body(PlSqlParser.Procedure_bodyContext context)
            {
                if (context == null) return null;

                // Extract procedure name
                var identifier = context.identifier();
                if (identifier != null)
                {
                    string procName = GetIdentifierText(identifier).ToUpperInvariant();
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = procName;

                    try
                    {
                        // Visit body which contains seq_of_statements
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        // Restore previous procedure name (for nested procedures)
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    // Visit body even if we can't get the name
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                // Don't call base.VisitProcedure_body to avoid double-visiting
                return null;
            }

            // Visit function body
            public override object VisitFunction_body(PlSqlParser.Function_bodyContext context)
            {
                if (context == null) return null;

                // Extract function name
                var identifier = context.identifier();
                if (identifier != null)
                {
                    string funcName = GetIdentifierText(identifier).ToUpperInvariant();
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = funcName;

                    try
                    {
                        // Visit body which contains seq_of_statements
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        // Restore previous procedure name (for nested functions)
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    // Visit body even if we can't get the name
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                // Don't call base.VisitFunction_body to avoid double-visiting
                return null;
            }

            // Visit CREATE PROCEDURE statements (standalone procedures)
            public override object VisitCreate_procedure_body(PlSqlParser.Create_procedure_bodyContext context)
            {
                if (context == null) return null;

                // Extract procedure name
                var procedureName = context.procedure_name();
                if (procedureName != null)
                {
                    string procName = GetProcedureNameText(procedureName);
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = procName;

                    try
                    {
                        // Visit body which contains seq_of_statements
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        // Restore previous procedure name
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    // Visit body even if we can't get the name
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                // Don't call base.VisitCreate_procedure_body to avoid double-visiting
                return null;
            }

            // Visit CREATE FUNCTION statements (standalone functions)
            public override object VisitCreate_function_body(PlSqlParser.Create_function_bodyContext context)
            {
                if (context == null) return null;

                // Extract function name
                var functionName = context.function_name();
                if (functionName != null)
                {
                    string funcName = GetFunctionNameText(functionName);
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = funcName;

                    try
                    {
                        // Visit body which contains seq_of_statements
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        // Restore previous procedure name
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    // Visit body even if we can't get the name
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                // Don't call base.VisitCreate_function_body to avoid double-visiting
                return null;
            }

            // Visit body (BEGIN ... END block)
            public override object VisitBody(PlSqlParser.BodyContext context)
            {
                if (context == null) return null;

                // Visit seq_of_statements which contains the actual DML statements
                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }

                // Don't call base.VisitBody to avoid double-visiting
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
                    return null; // Don't visit other children if we found sql_statement
                }

                // Visit loop_statement which may contain cursor FOR loops with SELECT statements
                var loopStmt = context.loop_statement();
                if (loopStmt != null)
                {
                    Visit(loopStmt);
                    return null; // Don't visit other children if we found loop_statement
                }

                // Visit block statements (nested BEGIN...END blocks)
                var block = context.block();
                if (block != null)
                {
                    Visit(block);
                    return null; // Don't visit other children if we found block
                }

                // Visit if_statement (may contain nested statements)
                var ifStmt = context.if_statement();
                if (ifStmt != null)
                {
                    Visit(ifStmt);
                    return null;
                }

                // Visit body (BEGIN...END block)
                var body = context.body();
                if (body != null)
                {
                    Visit(body);
                    return null;
                }

                // For other statement types, visit children to find nested statements
                // This handles assignment_statement, continue_statement, exit_statement, etc.
                // that might contain expressions with subqueries
                VisitChildren(context);

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
                if (context == null) return null;

                // Visit SELECT statement
                var selectStmt = context.select_statement();
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
                if (context == null) return null;

                var selectOnly = context.select_only_statement();
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
                if (context == null) return null;

                // Visit SELECT list (may contain subqueries in expressions)
                var selectedList = context.selected_list();
                if (selectedList != null)
                {
                    Visit(selectedList);
                }

                // Visit FROM clause
                var fromClause = context.from_clause();
                if (fromClause != null)
                {
                    Visit(fromClause);
                }

                // Visit WHERE clause (may contain subqueries with tables)
                var whereClause = context.where_clause();
                if (whereClause != null)
                {
                    Visit(whereClause);
                }

                // Don't call base.VisitQuery_block to avoid double-visiting
                return null;
            }

            // Visit WHERE clause to extract tables from subqueries
            public override object VisitWhere_clause(PlSqlParser.Where_clauseContext context)
            {
                if (context == null) return null;
                
                // Visit children to find subqueries and other expressions that may contain tables
                // This will automatically visit subqueries through the visitor pattern
                VisitChildren(context);
                
                return null;
            }

            // Visit loop statement (FOR loops, WHILE loops, etc.)
            public override object VisitLoop_statement(PlSqlParser.Loop_statementContext context)
            {
                if (context == null) return null;

                // Visit cursor_loop_param which may contain SELECT statements
                var cursorLoopParam = context.cursor_loop_param();
                if (cursorLoopParam != null)
                {
                    Visit(cursorLoopParam);
                }

                // Visit seq_of_statements inside the loop body
                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }

                // Don't call base.VisitLoop_statement to avoid double-visiting
                return null;
            }

            // Visit cursor loop parameter (FOR rec IN (SELECT ...) or FOR rec IN cursor_name)
            public override object VisitCursor_loop_param(PlSqlParser.Cursor_loop_paramContext context)
            {
                if (context == null) return null;

                // cursor_loop_param: record_name IN (cursor_name ('(' expressions_? ')')? | '(' select_statement ')')
                // Check for SELECT statement in parentheses (inline cursor)
                var selectStmt = context.select_statement();
                if (selectStmt != null)
                {
                    VisitSelect_statement(selectStmt);
                }

                // Don't call base.VisitCursor_loop_param to avoid double-visiting
                return null;
            }

            // Visit FROM clause
            public override object VisitFrom_clause(PlSqlParser.From_clauseContext context)
            {
                if (context == null) return null;

                var tableRefList = context.table_ref_list();
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
                if (context == null) return null;

                // Visit the main table reference
                var aux = context.table_ref_aux();
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
                if (context == null) return null;

                var tableRefInternal = context.table_ref_aux_internal();
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
                if (context == null) return null;

                var dmlTableExpr = context.dml_table_expression_clause();
                if (dmlTableExpr != null)
                {
                    Visit(dmlTableExpr);
                }
                else
                {
                    // If dml_table_expression_clause is null, visit children to find what's actually there
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
                if (context == null) return null;

                var tableviewName = context.tableview_name();
                if (tableviewName != null)
                {
                    ExtractTableName(tableviewName);
                    // Don't visit children if we found tableview_name directly
                    return null;
                }

                // Check for select_statement (wrapped in parentheses)
                var selectStmt = context.select_statement();
                if (selectStmt != null)
                {
                    VisitSelect_statement(selectStmt);
                    return null;
                }

                // Check for subqueries
                var subquery = context.subquery();
                if (subquery != null)
                {
                    VisitSubquery(subquery);
                    return null;
                }

                // If we get here, tableview_name, select_statement, and subquery are all null
                // Visit children to find what's actually there (might be table_collection_expression or other structures)
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

            // Do NOT override VisitTableview_name globally - we only want to extract tables from tableview_name
            // when they're in FROM clauses, not from qualified column references in SELECT expressions.
            // Table extraction from tableview_name is handled in VisitDml_table_expression_clause which
            // is only called from FROM clause contexts.

            // Extract table name from tableview_name context
            private void ExtractTableName(PlSqlParser.Tableview_nameContext context)
            {
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
                                    // Only add table if we should extract tables (filtering by procedure name)
                                    if (ShouldExtractTables())
                                    {
                                        _tableNames.Add(tableName);
                                    }
                                }
                            }
                            else
                            {
                                // Only add table if we should extract tables (filtering by procedure name)
                                if (ShouldExtractTables())
                                {
                                    _tableNames.Add(tableName);
                                }
                            }
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

            // Helper to get text from procedure_name (handles schema.procedure_name)
            private string GetProcedureNameText(PlSqlParser.Procedure_nameContext context)
            {
                if (context == null) return string.Empty;

                // Get the full text and normalize (remove extra whitespace)
                string fullText = context.GetText();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    return fullText.Trim().ToUpperInvariant();
                }

                // Fallback: try to get from identifier
                var identifier = context.identifier();
                if (identifier != null)
                {
                    return GetIdentifierText(identifier).ToUpperInvariant();
                }

                return string.Empty;
            }

            // Helper to get text from function_name (handles schema.function_name)
            private string GetFunctionNameText(PlSqlParser.Function_nameContext context)
            {
                if (context == null) return string.Empty;

                // Get the full text and normalize (remove extra whitespace)
                string fullText = context.GetText();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    return fullText.Trim().ToUpperInvariant();
                }

                // Fallback: try to get from identifier
                var identifier = context.identifier();
                if (identifier != null)
                {
                    return GetIdentifierText(identifier).ToUpperInvariant();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Custom visitor to extract procedure calls from the parse tree.
        /// </summary>
        private class ProcedureCallExtractorVisitor : PlSqlParserBaseVisitor<object>
        {
            private readonly HashSet<string> _procedureNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly string _targetProcedureName;
            private string _currentProcedureName;
            private string _currentPackageName;

            public HashSet<string> ProcedureNames => _procedureNames;

            public ProcedureCallExtractorVisitor(string targetProcedureName = null)
            {
                _targetProcedureName = targetProcedureName;
            }

            /// <summary>
            /// Checks if we should extract procedures in the current context.
            /// Returns true if no target procedure is specified, or if we're inside the target procedure.
            /// </summary>
            private bool ShouldExtractProcedures()
            {
                return string.IsNullOrWhiteSpace(_targetProcedureName) || 
                       (string.Equals(_currentProcedureName, _targetProcedureName, StringComparison.OrdinalIgnoreCase));
            }

            // Visit SQL script (entry point)
            public override object VisitSql_script(PlSqlParser.Sql_scriptContext context)
            {
                if (context == null) return null;
                return base.VisitSql_script(context);
            }

            // Visit anonymous blocks
            public override object VisitAnonymous_block(PlSqlParser.Anonymous_blockContext context)
            {
                if (context == null) return null;
                
                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }
                
                return null;
            }

            // Visit CREATE PACKAGE BODY statements - extract package name
            public override object VisitCreate_package_body(PlSqlParser.Create_package_bodyContext context)
            {
                if (context == null) return null;

                // Extract package name
                // Grammar: CREATE PACKAGE BODY [schema_object_name.]package_name
                // schema_object_name is optional schema name
                // package_name is the actual package name
                string packageName = null;
                var packageNames = context.package_name();
                if (packageNames != null && packageNames.Length > 0)
                {
                    // Get the actual package name (last one if multiple)
                    packageName = GetPackageNameText(packageNames[packageNames.Length - 1]);
                    
                    // Prepend schema if present
                    var schemaObjectName = context.schema_object_name();
                    if (schemaObjectName != null && !string.IsNullOrWhiteSpace(packageName))
                    {
                        string schema = GetSchemaObjectNameText(schemaObjectName);
                        if (!string.IsNullOrWhiteSpace(schema))
                        {
                            packageName = schema + "." + packageName;
                        }
                    }
                }

                string previousPackageName = _currentPackageName;
                if (!string.IsNullOrWhiteSpace(packageName))
                {
                    _currentPackageName = packageName.ToUpperInvariant();
                }

                try
                {
                    // Visit all package_obj_body elements (procedures, functions, etc.)
                    foreach (var packageObjBody in context.package_obj_body())
                    {
                        if (packageObjBody != null)
                        {
                            Visit(packageObjBody);
                        }
                    }

                    // Visit optional BEGIN seq_of_statements section (package initialization)
                    var seqOfStatements = context.seq_of_statements();
                    if (seqOfStatements != null)
                    {
                        Visit(seqOfStatements);
                    }
                }
                finally
                {
                    // Restore previous package name (for nested packages)
                    _currentPackageName = previousPackageName;
                }

                return null;
            }

            // Visit package object body
            public override object VisitPackage_obj_body(PlSqlParser.Package_obj_bodyContext context)
            {
                if (context == null) return null;

                var procedureBody = context.procedure_body();
                if (procedureBody != null)
                {
                    Visit(procedureBody);
                }

                var functionBody = context.function_body();
                if (functionBody != null)
                {
                    Visit(functionBody);
                }

                return null;
            }

            // Visit procedure body
            public override object VisitProcedure_body(PlSqlParser.Procedure_bodyContext context)
            {
                if (context == null) return null;

                var identifier = context.identifier();
                if (identifier != null)
                {
                    string procName = GetIdentifierText(identifier).ToUpperInvariant();
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = procName;

                    try
                    {
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                return null;
            }

            // Visit function body
            public override object VisitFunction_body(PlSqlParser.Function_bodyContext context)
            {
                if (context == null) return null;

                var identifier = context.identifier();
                if (identifier != null)
                {
                    string funcName = GetIdentifierText(identifier).ToUpperInvariant();
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = funcName;

                    try
                    {
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                return null;
            }

            // Visit CREATE PROCEDURE statements (standalone procedures)
            public override object VisitCreate_procedure_body(PlSqlParser.Create_procedure_bodyContext context)
            {
                if (context == null) return null;

                var procedureName = context.procedure_name();
                if (procedureName != null)
                {
                    string procName = GetProcedureNameText(procedureName);
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = procName;

                    try
                    {
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                return null;
            }

            // Visit CREATE FUNCTION statements (standalone functions)
            public override object VisitCreate_function_body(PlSqlParser.Create_function_bodyContext context)
            {
                if (context == null) return null;

                var functionName = context.function_name();
                if (functionName != null)
                {
                    string funcName = GetFunctionNameText(functionName);
                    string previousProcName = _currentProcedureName;
                    _currentProcedureName = funcName;

                    try
                    {
                        var body = context.body();
                        if (body != null)
                        {
                            Visit(body);
                        }
                    }
                    finally
                    {
                        _currentProcedureName = previousProcName;
                    }
                }
                else
                {
                    var body = context.body();
                    if (body != null)
                    {
                        Visit(body);
                    }
                }

                return null;
            }

            // Visit body (BEGIN ... END block)
            public override object VisitBody(PlSqlParser.BodyContext context)
            {
                if (context == null) return null;

                var seqOfStatements = context.seq_of_statements();
                if (seqOfStatements != null)
                {
                    Visit(seqOfStatements);
                }

                return null;
            }

            // Visit sequence of statements
            public override object VisitSeq_of_statements(PlSqlParser.Seq_of_statementsContext context)
            {
                if (context == null) return null;
                
                foreach (var statement in context.statement())
                {
                    Visit(statement);
                }
                
                return null;
            }

            // Visit statement - handle call_statement
            public override object VisitStatement(PlSqlParser.StatementContext context)
            {
                if (context == null) return null;

                // Visit call_statement if present
                var callStmt = context.call_statement();
                if (callStmt != null)
                {
                    VisitCall_statement(callStmt);
                    return null; // Don't visit children to avoid double-visiting
                }

                // Visit other statement types that may contain nested call_statements
                // Visit children to find nested statements
                VisitChildren(context);

                return null;
            }

            // Visit call statement - extract procedure calls
            public override object VisitCall_statement(PlSqlParser.Call_statementContext context)
            {
                if (context == null) return null;

                // Only extract if we should extract procedures (filtering by procedure name)
                if (!ShouldExtractProcedures())
                {
                    return null;
                }

                // call_statement contains one or more routine_name() separated by periods
                // Examples:
                // - proc_name() -> routine_name[0] = proc_name
                // - package.proc_name() -> routine_name[0] = package, routine_name[1] = proc_name
                // - schema.package.proc_name() -> routine_name[0] = schema.package, routine_name[1] = proc_name
                var routineNames = context.routine_name();
                if (routineNames != null && routineNames.Length > 0)
                {
                    // Build the full qualified name
                    var parts = new List<string>();

                    // First routine_name might be schema.package or just package
                    if (routineNames.Length > 0)
                    {
                        string firstRoutine = GetRoutineNameText(routineNames[0]);
                        if (!string.IsNullOrWhiteSpace(firstRoutine))
                        {
                            parts.Add(firstRoutine);
                        }
                    }

                    // Remaining routine_names are package.proc or just proc
                    for (int i = 1; i < routineNames.Length; i++)
                    {
                        string routine = GetRoutineNameText(routineNames[i]);
                        if (!string.IsNullOrWhiteSpace(routine))
                        {
                            parts.Add(routine);
                        }
                    }

                    if (parts.Count > 0)
                    {
                        string fullName = string.Join(".", parts);

                        // If unqualified call (single part) and we're inside a package, prepend package name
                        if (parts.Count == 1 && !string.IsNullOrWhiteSpace(_currentPackageName))
                        {
                            // Check if it's already qualified (contains dot from schema qualification)
                            if (!fullName.Contains("."))
                            {
                                fullName = _currentPackageName + "." + fullName;
                            }
                        }

                        _procedureNames.Add(fullName);
                    }
                }

                return null;
            }

            // Helper to get text from routine_name
            private string GetRoutineNameText(PlSqlParser.Routine_nameContext context)
            {
                if (context == null) return string.Empty;

                // routine_name: identifier ('.' id_expression)* ('@' link_name)?
                // The identifier is the base name, id_expression parts are schema qualification
                var identifier = context.identifier();
                if (identifier == null) return string.Empty;

                string baseName = GetIdentifierText(identifier);
                if (string.IsNullOrWhiteSpace(baseName)) return string.Empty;

                // Check for schema qualification (id_expression parts)
                var idExpressions = context.id_expression();
                if (idExpressions != null && idExpressions.Length > 0)
                {
                    // Build schema.package or schema.proc
                    var parts = new List<string> { baseName };
                    foreach (var idExpr in idExpressions)
                    {
                        string part = GetIdExpressionText(idExpr);
                        if (!string.IsNullOrWhiteSpace(part))
                        {
                            parts.Add(part);
                        }
                    }
                    return string.Join(".", parts);
                }

                return baseName;
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

                var delimitedId = context.DELIMITED_ID();
                if (delimitedId != null)
                {
                    string text = delimitedId.GetText();
                    if (text.Length >= 2 && text.StartsWith("\"") && text.EndsWith("\""))
                    {
                        text = text.Substring(1, text.Length - 2).Replace("\"\"", "\"");
                    }
                    return text;
                }

                var regularId = context.regular_id();
                if (regularId != null)
                {
                    var regularIdToken = regularId.REGULAR_ID();
                    if (regularIdToken != null)
                    {
                        return regularIdToken.GetText();
                    }
                    return regularId.GetText();
                }

                return string.Empty;
            }

            // Helper to get text from procedure_name
            private string GetProcedureNameText(PlSqlParser.Procedure_nameContext context)
            {
                if (context == null) return string.Empty;

                string fullText = context.GetText();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    return fullText.Trim().ToUpperInvariant();
                }

                var identifier = context.identifier();
                if (identifier != null)
                {
                    return GetIdentifierText(identifier).ToUpperInvariant();
                }

                return string.Empty;
            }

            // Helper to get text from function_name
            private string GetFunctionNameText(PlSqlParser.Function_nameContext context)
            {
                if (context == null) return string.Empty;

                string fullText = context.GetText();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    return fullText.Trim().ToUpperInvariant();
                }

                var identifier = context.identifier();
                if (identifier != null)
                {
                    return GetIdentifierText(identifier).ToUpperInvariant();
                }

                return string.Empty;
            }

            // Helper to get text from package_name
            private string GetPackageNameText(PlSqlParser.Package_nameContext context)
            {
                if (context == null) return string.Empty;

                var identifier = context.identifier();
                if (identifier != null)
                {
                    return GetIdentifierText(identifier);
                }

                return string.Empty;
            }

            // Helper to get text from schema_object_name
            private string GetSchemaObjectNameText(PlSqlParser.Schema_object_nameContext context)
            {
                if (context == null) return string.Empty;

                // schema_object_name contains id_expression (which can be schema.package or just package)
                // Get the full text
                string fullText = context.GetText();
                if (!string.IsNullOrWhiteSpace(fullText))
                {
                    return fullText.Trim();
                }

                // Try to extract from id_expression
                var idExpression = context.id_expression();
                if (idExpression != null)
                {
                    return GetIdExpressionText(idExpression);
                }

                return string.Empty;
            }
        }
    }
}
