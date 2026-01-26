---
name: Fix PL/SQL Table Name Extractor
overview: Fix the table name extractor to properly extract source tables from PL/SQL code, excluding target tables and properly handling CTE definitions and nested structures.
todos:
  - id: exclude_target_tables
    content: Remove target table extraction from INSERT, UPDATE, DELETE, and MERGE statements
    status: completed
  - id: visit_cte_definitions
    content: Enhance VisitWithClause to visit CTE definition subqueries and extract source tables
    status: completed
  - id: test_extraction
    content: Test with example SQL to verify all source tables are extracted correctly
    status: completed
isProject: false
---

# Fix PL/SQL Table Name Extractor

## Problem Analysis

The current implementation has three main issues:

1. **Target tables are being extracted**: INSERT/UPDATE/DELETE statements extract target table names (e.g., `rr_temp`), but we only need source tables from SELECT statements.

2. **CTE definitions are not visited**: The `VisitWithClause` method only collects CTE names but doesn't visit the CTE definition subqueries to extract source tables from them. According to the grammar, `subquery_factoring_clause` has the structure: `query_name ... AS '(' subquery ... ')'` - we need to visit the `subquery` part.

3. **Nested WITH clauses**: CTEs can contain nested WITH clauses that need recursive processing.

## Expected Results

For the example SQL in `Program.cs`, the extractor should return:

- `rr_rep_po` (line 145)
- `rr_rep_dog_obj` (line 156)  
- `rs_esys` (line 349)
- `kr_org` (line 364)
- `kr_payer` (line 381)
- `k_house` (line 391)
- `adr_m` (line 330)

It should NOT return:

- `rr_temp` (target table from INSERT/DELETE)
- `mat1` (CTE name)

## Implementation Plan

### 1. Exclude Target Tables from DML Statements

**File**: `SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`

- **VisitInsert_statement**: Remove extraction of target table from `insert_into_clause`. Keep visiting the SELECT statement.
- **VisitUpdate_statement**: Remove extraction of target table. Keep visiting any subqueries in WHERE clauses.
- **VisitDelete_statement**: Remove extraction of target table. Keep visiting any subqueries in WHERE clauses.
- **VisitMerge_statement**: Remove extraction of target table from INTO clause. Keep visiting the USING clause (which contains source tables).

### 2. Visit CTE Definitions to Extract Source Tables

**File**: `SqlBuilderLib/DevTools/DevSqlParserAntlr.cs`

- **VisitWithClause**: After collecting CTE names, visit each `subquery_factoring_clause` to extract source tables from the CTE definition subqueries.
- **VisitSubquery_factoring_clause**: Add a new visitor method that:
  - Collects the CTE name (already done)
  - Visits the `subquery` part of the CTE definition to extract source tables recursively
  - Handles nested WITH clauses within CTE definitions

### 3. Ensure Proper Recursive Processing

- Verify that nested subqueries in CTEs are properly visited
- Ensure that CTE names are tracked per SELECT statement scope (not globally) to handle cases where the same name might be reused in different scopes
- Make sure UNION statements within CTEs are properly handled

## Code Changes

### Change 1: Modify VisitInsert_statement

Remove the code that extracts table names from `insert_into_clause` (lines 117-133). Keep only the SELECT statement visiting logic.

### Change 2: Modify VisitUpdate_statement  

Remove the code that extracts table names from the UPDATE target (lines 184-196). Keep visiting subqueries.

### Change 3: Modify VisitDelete_statement

Remove the code that extracts table names from the DELETE target (lines 206-218). Keep visiting subqueries.

### Change 4: Modify VisitMerge_statement

Remove the code that extracts table names from the INTO clause (lines 229-233). Keep visiting the USING clause.

### Change 5: Enhance VisitWithClause

After collecting CTE names, visit each `subquery_factoring_clause` to process the CTE definition subqueries:

```csharp
private void VisitWithClause(PlSqlParser.With_clauseContext context)
{
    if (context == null) return;

    foreach (var factoring in context.with_factoring_clause())
    {
        var subqueryFactoring = factoring.subquery_factoring_clause();
        if (subqueryFactoring != null)
        {
            // Collect CTE name (existing logic)
            // ...
            
            // NEW: Visit the CTE definition subquery to extract source tables
            var cteSubquery = subqueryFactoring.subquery();
            if (cteSubquery != null)
            {
                VisitSubquery(cteSubquery);
            }
        }
    }
}
```

### Change 6: Add VisitSubquery_factoring_clause method (if needed)

If the grammar structure requires it, add a dedicated visitor method for `subquery_factoring_clause` that handles both CTE name collection and subquery visiting.

## Testing

After implementation, test with the example SQL in `Program.cs` to verify:

- All source tables are extracted
- Target tables are excluded
- CTE names are excluded
- Nested structures work correctly