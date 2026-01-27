---
name: PL/SQL Procedure Call Extractor
overview: Add extraction of stored procedure calls from PL/SQL code, handling package-qualified calls, standalone procedures, and same-package calls that need package name resolution.
todos:
  - id: add_procedure_extractor_visitor
    content: Create ProcedureCallExtractorVisitor class with package name tracking and call_statement handling
    status: completed
  - id: implement_package_tracking
    content: Add _currentPackageName tracking and extract package name in VisitCreate_package_body
    status: completed
  - id: implement_call_extraction
    content: Override VisitCall_statement to extract routine_name and build qualified procedure names
    status: completed
  - id: add_helper_methods
    content: Add GetRoutineNameText helper method to extract full routine names from Routine_nameContext
    status: completed
  - id: add_public_method
    content: Add public GetSourceProcedures method following same pattern as GetSourceTables
    status: completed
  - id: handle_same_package_resolution
    content: Implement logic to prepend package name to unqualified procedure calls within same package
    status: completed
isProject: false
---

# PL/SQL Procedure Call Extractor Implementation Plan

## Overview

Add functionality to extract stored procedure calls from PL/SQL code, similar to the existing table name extraction. The implementation will handle:

- Package-qualified calls: `package_name.proc_name(...)`
- Standalone procedures: `proc_name(...)`
- Same-package calls: unqualified calls within a package should be resolved to `package_name.proc_name`

## Implementation Approach

### Method Structure

Add a new public method `GetSourceProcedures` to `DevSqlParserAntlr` class in [`SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`](SqlBuilderLib/DevTools/DevSqlParserAntlr.cs):

```csharp
public static HashSet<string> GetSourceProcedures(string plsqlText, string procedureName = null)
```

This follows the same pattern as `GetSourceTables` for consistency and minimal code changes.

### Key Implementation Details

1. **Reuse Existing Visitor Pattern**

   - Create a new visitor class `ProcedureCallExtractorVisitor` similar to `TableNameExtractorVisitor`
   - Reuse the same package/procedure name tracking logic (`_currentProcedureName`, `_currentPackageName`)

2. **Track Package Context**

   - Add `_currentPackageName` field to track when we're inside a package body
   - Update `VisitCreate_package_body` to extract and track package name
   - When visiting `call_statement`, if procedure name is unqualified and we're inside a package, prepend package name

3. **Extract Procedure Calls**

   - Override `VisitCall_statement` in the visitor
   - Extract `routine_name()` from `call_statement` context
   - Handle multiple `routine_name()` separated by periods (for package.proc syntax)
   - Build full qualified name: `schema.package.proc` or `package.proc` or `proc` (with package prefix if inside package)

4. **Handle Routine Name Structure**

   - `routine_name()` can contain:
     - Single identifier: `proc_name`
     - Schema-qualified: `schema.proc_name` (via id_expression)
     - Package-qualified: `package.proc_name` (multiple routine_name with periods)
   - Extract using existing helper methods: `GetIdentifierText`, `GetIdExpressionText`

5. **Package Name Resolution**

   - When inside a package body and encountering unqualified procedure call, prepend current package name
   - Example: Inside `MY_PACKAGE`, call `proc_name()` → extract as `MY_PACKAGE.PROC_NAME`

## Files to Modify

### [`SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`](SqlBuilderLib/DevTools/DevSqlParserAntlr.cs)

1. **Add new public method** `GetSourceProcedures` (similar structure to `GetSourceTables`)
2. **Add new visitor class** `ProcedureCallExtractorVisitor`:

   - Track `_currentPackageName` (in addition to `_currentProcedureName`)
   - Override `VisitCreate_package_body` to extract package name
   - Override `VisitCall_statement` to extract procedure calls
   - Override `VisitRoutine_name` helper to build qualified names
   - Reuse existing helper methods for identifier extraction

3. **Helper method** `GetRoutineNameText`:

   - Extract full routine name from `Routine_nameContext`
   - Handle schema qualification, package qualification, and database links

## Grammar Context Understanding

From the generated parser:

- `call_statement`: Contains optional `CALL` keyword, then one or more `routine_name()` separated by periods
- `routine_name`: Contains `identifier()` and optional `id_expression()` parts (for schema qualification)
- Multiple `routine_name()` with periods = package.proc syntax

## Edge Cases to Handle

- Procedure calls in anonymous blocks (no package context)
- Procedure calls in standalone procedures (no package context)
- Procedure calls in package bodies (need package name resolution)
- Nested packages (track package name stack)
- Schema-qualified calls: `schema.package.proc` or `schema.proc`
- Database links: `package.proc@dblink` (extract without dblink)
- Function calls vs procedure calls (both use `call_statement`)

## Testing Considerations

Test with:

- Simple procedure call: `proc_name();`
- Package-qualified: `package.proc_name();`
- Schema-qualified: `schema.proc_name();`
- Same-package call: Inside `MY_PACKAGE`, call `proc_name()` → should extract `MY_PACKAGE.PROC_NAME`
- Nested packages
- Anonymous blocks with procedure calls
- Standalone procedures calling other procedures