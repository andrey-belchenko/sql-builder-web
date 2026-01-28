---
name: Create DevSqlParserCustom
overview: Create a new DevSqlParserCustom class with GetSourceTables and GetSourcePackages methods that extract Latin words from SQL and match them against database metadata loaded on initialization.
todos:
  - id: create_class
    content: Create DevSqlParserCustom.cs file with class structure and namespace
    status: completed
  - id: implement_metadata_loading
    content: Implement lazy loading of tables/views and packages from database using all_tables, all_views, and all_objects
    status: completed
  - id: implement_word_extraction
    content: Implement regex-based Latin word extraction from SQL text
    status: completed
  - id: implement_get_source_tables
    content: Implement GetSourceTables method that matches extracted words against table/view names
    status: completed
  - id: implement_get_source_packages
    content: Implement GetSourcePackages method that matches extracted words against package names
    status: completed
isProject: false
---

# Create DevSqlParserCustom Class

## Overview

Create a new static class `DevSqlParserCustom` in `SqlBuilderLib/DevTools/` that provides simpler custom logic for extracting table/view and package names from SQL by matching Latin words against database metadata.

## Implementation Details

### File Location

- Create: `SqlBuilderLib/DevTools/DevSqlParserCustom.cs`

### Class Structure

- Static class similar to `DevSqlParserAntlr`
- Lazy initialization: Load database metadata on first use
- Store metadata in `HashSet<string>` collections for fast lookup

### Database Metadata Loading

On first use, execute queries to load:

1. **Tables and Views**: 
   ```sql
   SELECT table_name FROM all_tables
   UNION
   SELECT view_name FROM all_views
   ```


Store in `_tableAndViewNames` HashSet

2. **Packages**:
   ```sql
   SELECT object_name FROM all_objects 
   WHERE object_type = 'PACKAGE' OR object_type = 'PACKAGE BODY'
   ```


Store in `_packageNames` HashSet

Use `db.Connection` and `DataHelper.SqlGetTable()` similar to patterns in `db.cs` and `XmlSchemeBuilder.cs`.

### Methods

#### `GetSourceTables(string plsqlText)`

1. Extract all Latin words from SQL using regex pattern: `\b[A-Za-z][A-Za-z0-9_]*\b`
2. Match extracted words (case-insensitive) against `_tableAndViewNames`
3. Return `HashSet<string>` of matching names

#### `GetSourcePackages(string plsqlText)`

1. Extract all Latin words from SQL using same regex pattern
2. Match extracted words (case-insensitive) against `_packageNames`
3. Return `HashSet<string>` of matching names

### Word Extraction Logic

- Use `Regex.Matches()` with pattern `\b[A-Za-z][A-Za-z0-9_]*\b` to find all Latin identifiers
- Filter out SQL keywords (optional, but may improve accuracy)
- Normalize to uppercase for case-insensitive comparison

### Thread Safety

- Use `Lazy<T>` or static initialization with lock for thread-safe lazy loading
- Consider using `LazyInitializer.EnsureInitialized()` pattern

## Files to Create

- `SqlBuilderLib/DevTools/DevSqlParserCustom.cs`

## Files to Reference

- `SqlBuilderLib/DevTools/DevSqlParserAntlr.cs` - for method signatures and structure
- `SqlBuilderLib/db.cs` - for database connection pattern
- `SqlBuilderLib/XmlHelpers/XmlSchemeBuilder.cs` - for Oracle metadata query examples