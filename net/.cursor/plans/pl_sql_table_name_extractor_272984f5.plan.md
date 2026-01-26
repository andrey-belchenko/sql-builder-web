---
name: PL/SQL Table Name Extractor
overview: Implement DevSqlParser static class with GetSourceTables method that extracts database table names from Oracle PL/SQL 11g code, handling anonymous blocks, procedures, packages, CTEs, subqueries, and UNION statements.
todos:
  - id: research_complete
    content: Research Oracle PL/SQL 11g parsers for .NET - COMPLETED
    status: pending
  - id: choose_approach
    content: Decide on parser approach (ANTLR vs Commercial vs Custom)
    status: completed
  - id: setup_dependencies
    content: Add required NuGet packages (Antlr4.Runtime.Standard, optionally Antlr4BuildTasks or AntlrOraclePlsql)
    status: completed
  - id: create_class_structure
    content: Create DevSqlParser.cs with GetSourceTables public method
    status: completed
  - id: implement_plsql_extraction
    content: Implement extraction of SELECT statements from PL/SQL (handling blocks, procedures, packages)
    status: completed
  - id: implement_select_analysis
    content: Implement SELECT statement analysis (CTEs, subqueries, UNION)
    status: completed
  - id: implement_table_extraction
    content: Implement table name extraction from FROM/JOIN clauses
    status: completed
  - id: handle_edge_cases
    content: Handle edge cases (aliases, schema qualification, comments, string literals)
    status: completed
  - id: test_implementation
    content: Test with various PL/SQL samples (simple SELECTs, CTEs, procedures, packages)
    status: completed
isProject: false
---

# PL/SQL Table Name Extractor Implementation Plan

## Research Summary

After researching Oracle PL/SQL 11g parsers for .NET, three viable approaches were identified:

### Option 1: ANTLR-Based Parser (Recommended)

- **AntlrOraclePlsql** library (dbobylev) - ready-to-use .NET library for PL/SQL 11g
- Uses official ANTLR grammars-v4 PL/SQL grammar
- Supports anonymous blocks, procedures, packages, functions
- Requires case-insensitive character stream wrapper
- **Pros**: Open source, full PL/SQL support, no licensing costs
- **Cons**: Requires understanding ANTLR parse trees, need to implement table extraction logic

### Option 2: General SQL Parser (Commercial)

- Commercial library with ~85%+ PL/SQL coverage
- Built-in table name extraction capabilities
- .NET Standard 2.0 compatible
- **Pros**: Mature, built-in table extraction, handles edge cases
- **Cons**: Requires license purchase, may not cover all edge cases

### Option 3: Custom Regex-Based Parser

- Build custom parser using regex patterns
- **Pros**: Full control, no dependencies
- **Cons**: Error-prone, difficult to handle all PL/SQL complexities, maintenance burden

## Recommended Approach: ANTLR-Based Solution

**Target Implementation: ANTLR**

Use **AntlrOraclePlsql** or compile ANTLR grammars directly, implementing custom table extraction logic. The implementation will use ANTLR parse trees to traverse SQL structures and extract table names.

## Implementation Details

### File Structure

- Create `SqlBuilderLib/DevTools/DevSqlParser.cs`
- Follow existing `DevAnalyzer.cs` pattern (internal static class)

### Core Method Signature

```csharp
public static HashSet<string> GetSourceTables(string plsqlText)
```

### Implementation Strategy

1. **Extract SELECT Statements**

   - Parse PL/SQL to identify all SELECT statements
   - Handle nested contexts: anonymous blocks, procedures, functions, packages

2. **Analyze Each SELECT Statement**

   - Parse SELECT statement structure using ANTLR
   - Handle CTEs (WITH clauses) - track CTE names but exclude from final results
   - **CRITICAL: Handle nested WITH clauses** - CTEs and subqueries can contain nested WITH clauses that must be processed recursively
   - Handle subqueries recursively (subqueries may contain their own WITH clauses)
   - Handle UNION/UNION ALL statements
   - Extract table names from:
     - FROM clauses
     - JOIN clauses (INNER, LEFT, RIGHT, FULL OUTER, CROSS)
     - UPDATE statements (in FROM clause)
     - MERGE statements

3. **Table Name Extraction Logic**

   - Extract from `table_name`, `schema.table_name`, `schema.table_name@dblink`
   - Handle table aliases (ignore alias, use actual table name)
   - Handle subqueries (recurse into them)
   - Exclude CTE names from results
   - Handle DUAL and other system tables (configurable)

### Private Helper Methods

- `ExtractSelectStatements(string plsql)` - Extract all SELECT statements from PL/SQL
- `ExtractTablesFromSelect(string selectSql)` - Extract tables from a single SELECT
- `ExtractTablesFromFromClause(string fromClause)` - Parse FROM clause
- `ExtractTablesFromJoinClause(string joinClause)` - Parse JOIN clauses
- `ExtractCteNames(string selectSql)` - Identify CTE names to exclude
- `IsCteName(string name, HashSet<string> cteNames)` - Check if name is a CTE
- `NormalizeTableName(string tableName)` - Handle schema qualification, remove quotes
- `RemoveComments(string sql)` - Strip SQL comments
- `HandleExecuteImmediate(string plsql)` - Extract SQL from EXECUTE IMMEDIATE

### Dependencies

**If using AntlrOraclePlsql:**

- Reference the library or integrate grammar files
- Add `Antlr4.Runtime.Standard` NuGet package

**If compiling ANTLR grammars directly:**

- Add `Antlr4BuildTasks` NuGet package (for build-time grammar compilation)
- Add `Antlr4.Runtime.Standard` NuGet package
- Copy `PlSqlLexer.g4` and `PlSqlParser.g4` from antlr/grammars-v4

### Edge Cases to Handle

- **Nested WITH clauses** - CTEs can contain nested WITH clauses, subqueries can contain WITH clauses
- Nested subqueries in SELECT, FROM, WHERE, HAVING clauses (may contain their own WITH clauses)
- CTEs with multiple CTE definitions
- UNION statements combining multiple SELECTs (each may have WITH clauses)
- Table aliases vs actual table names
- Schema-qualified names (SCHEMA.TABLE)
- Database links (TABLE@DBLINK)
- Quoted identifiers
- Comments (single-line -- and multi-line /* */)
- String literals containing SQL keywords
- PL/SQL variables vs table names

### Testing Considerations

- Test with simple SELECT statements
- Test with CTEs
- Test with nested subqueries
- Test with UNION statements
- Test with procedures containing SELECTs
- Test with anonymous blocks
- Test with packages
- Test with EXECUTE IMMEDIATE
- Test with complex joins
- Test with schema-qualified names

## Decision Required

Choose one of the following approaches:

1. **ANTLR-based** (recommended) - Use AntlrOraclePlsql or compile grammars
2. **General SQL Parser** - Commercial library with built-in extraction
3. **Custom parser** - Regex-based, more control but more complex

The plan assumes ANTLR-based approach unless specified otherwise.