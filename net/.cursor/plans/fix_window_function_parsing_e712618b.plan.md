---
name: Fix Window Function Parsing
overview: Regenerate ANTLR parser from updated grammar to support custom functions with window functions (over_clause), then rebuild and test to fix the parsing error for stragg_dist() with over() clause.
todos:
  - id: locate-antlr
    content: Locate ANTLR jar file in common locations or download if missing
    status: in_progress
  - id: regenerate-parser
    content: Run regenerate_antlr.ps1 script to regenerate parser files from updated grammar
    status: pending
  - id: rebuild-generated
    content: Build SqlBuilderLib.Generated project to compile updated parser
    status: pending
  - id: rebuild-main
    content: Build SqlBuilderApp project to link with updated parser
    status: pending
  - id: test-fix
    content: Run test program and verify parsing succeeds for Sql/1.sql without errors
    status: pending
  - id: verify-logs
    content: Check debug logs if needed to confirm parser behavior
    status: pending
isProject: false
---

# Fix Window Function Parsing Issue

## Problem

The parser fails when encountering custom functions (like `stragg_dist`) with window functions (`over()` clause) at line 1358 in `Sql/1.sql`. The grammar has been updated but the generated parser files are outdated.

## Current State

**Fixed:**

- Grammar file [`SqlBuilderLib/Grammars/PlSqlParser.g4`](SqlBuilderLib/Grammars/PlSqlParser.g4) - Added support for custom functions with `over_clause` (lines 6951-6952)
- Debug instrumentation added to [`SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`](SqlBuilderLib/DevTools/DevSqlParserAntlr.cs)

**Needs Update:**

- Generated parser files in `SqlBuilderLib.Generated/` are outdated and don't include the new grammar rules

## Solution Steps

### Step 1: Locate or Download ANTLR Tool

- Check common locations for `antlr-4.13.1-complete.jar`:
  - Project root: `net/antlr-4.13.1-complete.jar`
  - User home: `$env:USERPROFILE\.antlr\antlr-4.13.1-complete.jar`
  - Local app data: `$env:LOCALAPPDATA\antlr\antlr-4.13.1-complete.jar`
  - Tools directory: `C:\tools\antlr\antlr-4.13.1-complete.jar`
- If not found, download from: https://github.com/antlr/antlr4/releases/download/4.13.1/antlr-4.13.1-complete.jar
- Save to project root as `antlr-4.13.1-complete.jar`

### Step 2: Regenerate Parser Files

- Run the regeneration script: `.\regenerate_antlr.ps1`
- Or manually execute:
  ```powershell
  java -cp "antlr-4.13.1-complete.jar" org.antlr.v4.Tool `
    -Dlanguage=CSharp `
    -o SqlBuilderLib.Generated `
    SqlBuilderLib/Grammars/PlSqlLexer.g4 `
    SqlBuilderLib/Grammars/PlSqlParser.g4
  ```

- This generates updated `PlSqlParser.cs`, `PlSqlLexer.cs`, and related files in `SqlBuilderLib.Generated/`

### Step 3: Rebuild Generated Project

- Build the generated project: `dotnet build SqlBuilderLib.Generated\SqlBuilderLib.Generated.csproj`
- Verify build succeeds without errors

### Step 4: Rebuild Main Project

- Build the main application: `dotnet build SqlBuilderApp\SqlBuilderApp.csproj`
- Verify build succeeds

### Step 5: Test the Fix

- Clear debug log: Delete `.cursor/debug.log` if it exists
- Run the test program (executes `Program.TestSqlParsing`)
- Verify that parsing succeeds for `Sql/1.sql` without the "no viable alternative" error
- Check that `stragg_dist(...) over(...)` is parsed correctly

### Step 6: Verify with Logs (if needed)

- If errors persist, check `.cursor/debug.log` for detailed parser error information
- Use log data to diagnose any remaining issues

## Files Modified

- [`SqlBuilderLib/Grammars/PlSqlParser.g4`](SqlBuilderLib/Grammars/PlSqlParser.g4) - Already updated with new rules
- [`SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`](SqlBuilderLib/DevTools/DevSqlParserAntlr.cs) - Already has debug instrumentation

## Files Generated (by ANTLR)

- `SqlBuilderLib.Generated/PlSqlParser.cs` - Will be regenerated
- `SqlBuilderLib.Generated/PlSqlLexer.cs` - Will be regenerated
- `SqlBuilderLib.Generated/PlSqlParserBase.cs` - Will be regenerated
- `SqlBuilderLib.Generated/PlSqlParserVisitor.cs` - Will be regenerated
- `SqlBuilderLib.Generated/PlSqlParserBaseVisitor.cs` - Will be regenerated

## Expected Outcome

After regeneration, the parser should recognize the pattern:

```
stragg_dist( ur_isp_other_prim.prim ) over( partition by ur_isp.kod_isp order by ur_isp_other_prim.kod_isp_other_prim ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )
```

The `other_function` rule will match `regular_id function_argument over_clause?` alternative, allowing custom functions to use window functions.