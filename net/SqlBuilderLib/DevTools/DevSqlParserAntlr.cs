using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                // Add custom error listener to catch parse errors
                parser.AddErrorListener(new ThrowingErrorListener());

                // Parse the input
                var tree = parser.sql_script();

                // Create visitor to extract table names
                var visitor = new TableNameExtractorVisitor();
                visitor.Visit(tree);

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
        /// Error listener that throws exceptions on parse errors.
        /// </summary>
        private class ThrowingErrorListener : Antlr4.Runtime.BaseErrorListener
        {
            public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                throw new InvalidOperationException($"Parse error at line {line}, position {charPositionInLine}: {msg}", e);
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
                // Use VisitChildren to automatically visit all children
                return base.VisitAnonymous_block(context);
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

                return null;
            }

            // Visit UPDATE statements
            public override object VisitUpdate_statement(PlSqlParser.Update_statementContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table from UPDATE statement
                // Only visit subqueries in WHERE clauses, SET expressions, etc.
                return base.VisitUpdate_statement(context);
            }

            // Visit DELETE statements
            public override object VisitDelete_statement(PlSqlParser.Delete_statementContext context)
            {
                if (context == null) return null;

                // Do NOT extract target table from DELETE statement
                // Only visit subqueries in WHERE clauses, etc.
                return base.VisitDelete_statement(context);
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

                return base.VisitMerge_statement(context);
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

                return null;
            }

            // Visit query block to extract table names from FROM and JOIN clauses
            public override object VisitQuery_block(PlSqlParser.Query_blockContext context)
            {
                if (context == null) return null;

                // Visit FROM clause
                var fromClause = context.from_clause();
                if (fromClause != null)
                {
                    Visit(fromClause);
                }

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
                    VisitChildren(tableRefInternal);
                }

                return null;
            }

            // Override visitor methods for labeled alternatives of table_ref_aux_internal
            public override object VisitTable_ref_aux_internal_one(PlSqlParser.Table_ref_aux_internal_oneContext context)
            {
                if (context == null) return null;

                var dmlTableExpr = context.dml_table_expression_clause();
                if (dmlTableExpr != null)
                {
                    var tableviewName = dmlTableExpr.tableview_name();
                    if (tableviewName != null)
                    {
                        ExtractTableName(tableviewName);
                    }

                    // Check for subqueries
                    var subquery = dmlTableExpr.subquery();
                    if (subquery != null)
                    {
                        VisitSubquery(subquery);
                    }
                }

                return base.VisitTable_ref_aux_internal_one(context);
            }

            public override object VisitTable_ref_aux_internal_two(PlSqlParser.Table_ref_aux_internal_twoContext context)
            {
                if (context == null) return null;

                var tableRef = context.table_ref();
                if (tableRef != null)
                {
                    Visit(tableRef);
                }

                return base.VisitTable_ref_aux_internal_two(context);
            }

            public override object VisitTable_ref_aux_internal_thre(PlSqlParser.Table_ref_aux_internal_threContext context)
            {
                if (context == null) return null;

                var dmlTableExpr = context.dml_table_expression_clause();
                if (dmlTableExpr != null)
                {
                    var tableviewName = dmlTableExpr.tableview_name();
                    if (tableviewName != null)
                    {
                        ExtractTableName(tableviewName);
                    }

                    var subquery = dmlTableExpr.subquery();
                    if (subquery != null)
                    {
                        VisitSubquery(subquery);
                    }
                }

                return base.VisitTable_ref_aux_internal_thre(context);
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

                return null;
            }

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
                                    _tableNames.Add(tableName);
                                }
                            }
                            else
                            {
                                _tableNames.Add(tableName);
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
        }
    }
}
