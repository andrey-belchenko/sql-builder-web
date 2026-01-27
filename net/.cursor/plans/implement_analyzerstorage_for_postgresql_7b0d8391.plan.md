---
name: Implement AnalyzerStorage for PostgreSQL
overview: Create a static AnalyzerStorage class that saves AnalyzerDependency and AnalyzerReportInfo collections to PostgreSQL tables using raw SQL and Npgsql, along with DDL files for table creation.
todos:
  - id: add-npgsql-package
    content: Add Npgsql NuGet package reference to SqlBuilderLib.csproj
    status: completed
  - id: create-sql-folder
    content: Create Sql folder in DevTools directory
    status: completed
  - id: create-dependencies-ddl
    content: Create dependencies.sql DDL file for report_dev_sqlb.dependencies table
    status: completed
  - id: create-reports-ddl
    content: Create reports.sql DDL file for report_dev_sqlb.reports table
    status: completed
  - id: create-analyzer-storage
    content: Create AnalyzerStorage.cs static class with SaveDependencies and SaveReports methods
    status: completed
isProject: false
---

## Implementation Plan

### 1. Add Npgsql NuGet Package

- Add `Npgsql` package reference to `SqlBuilderLib.csproj`

### 2. Create DDL Files

Create SQL DDL files in `net/SqlBuilderLib/DevTools/Sql/`:

- `dependencies.sql` - DDL for `report_dev_sqlb.dependencies` table
- `reports.sql` - DDL for `report_dev_sqlb.reports` table

Tables structure:

- **dependencies**: object_name, object_type, used_object_name, used_object_type (all VARCHAR/TEXT)
- **reports**: name, title, path, nav_id, nav_info (all VARCHAR/TEXT)

### 3. Create AnalyzerStorage Class

Create `net/SqlBuilderLib/DevTools/AnalyzerStorage.cs` with:

- Static class with hardcoded connection string
- `SaveDependencies(IEnumerable<AnalyzerDependency>)` method - bulk INSERT using parameterized queries
- `SaveReports(IEnumerable<AnalyzerReportInfo>)` method - bulk INSERT using parameterized queries
- Simple connection handling with using statements
- No error handling or logging (as requested)

### Implementation Details

- Connection string format: `Host=asusejs-dev.infoenergo.loc;Port=5432;Database=asuse;Username=asuse;Password=kl0pik`
- Use NpgsqlConnection and NpgsqlCommand
- Parameterized INSERT statements to prevent SQL injection
- Batch inserts for efficiency
- Synchronous methods (simplest approach)